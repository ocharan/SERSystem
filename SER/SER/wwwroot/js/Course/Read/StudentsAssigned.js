window.onload = function () {
	let studentsAssigned = localStorage.getItem("assignedStudents");
	let professorAssigned = localStorage.getItem("assignedProfessor");
	let removedStudent = localStorage.getItem("removedStudent");

	if (studentsAssigned) {
		showToast("Alumnos asignados correctamente", "assignedStudents");
	}

	if (professorAssigned) {
		showToast("Profesor asignado correctamente", "assignedProfessor");
	}

	if (removedStudent) {
		showToast("Alumno removido correctamente (Baja)", "removedStudent");
	}
};

function showToast(message, localStorageItem) {
	const toasBody = document.getElementById("toast-content");
	toasBody.innerHTML = "";
	const content = document.createTextNode(message);
	toasBody.appendChild(content);
	toast.show();

	localStorage.removeItem(localStorageItem);
}

const successToast = document.getElementById("toast-success");
const errorToast = document.getElementById("toast-error");
const toast = bootstrap.Toast.getOrCreateInstance(successToast);
const toastError = bootstrap.Toast.getOrCreateInstance(errorToast);
const urlParams = new URLSearchParams(window.location.search);
const courseId = urlParams.get("courseId"); // ProfessorAsignment.js
const studentList = document.getElementById("student-list");
const modalAassignStudents = new mdb.Modal(
	document.getElementById("modal-add-student")
);

const submitStudents = document.getElementById("button-submit-students");

submitStudents.addEventListener("click", function () {
	if (assignedStudents.length == 0) {
		const toasBody = document.getElementById("toast-content");
		toasBody.innerHTML = "";
		const content = document.createTextNode(
			"No ha seleccionado a ningún alumno"
		);
		toasBody.appendChild(content);
		toast.show();
	} else {
		const courseRegistrations = assignedStudents.map((studentId) =>
			createCourseRegistration(studentId)
		);

		const jsonContent = JSON.stringify(courseRegistrations);

		$.ajax({
			type: "POST",
			url: "?handler=CreateCourseRegistrations",
			contentType: "application/json",
			data: jsonContent,
			headers: {
				RequestVerificationToken: $(
					'input:hidden[name="__RequestVerificationToken"]'
				).val(),
			},
			success: function () {
				location.reload();
				localStorage.setItem("assignedStudents", "true");
			},
			error: function () {
				const toasBody = document.getElementById("toast-error-content");
				toasBody.innerHTML = "";
				const content = document.createTextNode(
					"Ha ocurrido un error al asignar los estudiantes al curso."
				);
				toasBody.appendChild(content);
				toastError.show();
				toast.hide();
			},
		});
	}
});

function createCourseRegistration(studentId) {
	let courseRegistration = {
		CourseId: courseId,
		StudentId: studentId,
	};

	return courseRegistration;
}

const buttonCloseStudents = document.getElementById("button-close-students");

buttonCloseStudents.addEventListener("click", function () {
	clearList();
});

function clearList() {
	assignedStudents = [];
	const selectedStudents = document.getElementById("container-students");
	selectedStudents.innerHTML = "";
	inputStudentSearch.value = "";
	studentList.innerHTML = "";
}
