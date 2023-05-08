// window.onload = function () {
// 	let studentsAssigned = localStorage.getItem("assignedStudents");

// 	if (studentsAssigned) {
// 		const toasBody = document.getElementById("toast-content");
// 		toasBody.innerHTML = "";
// 		const content = document.createTextNode("Alumnos asignados correctamente");
// 		toasBody.appendChild(content);
// 		toast.show();

// 		localStorage.removeItem("assignedStudents");
// 	}
// };

window.onload = function () {
	let studentsAssigned = localStorage.getItem("assignedStudents");
	let professorAssigned = localStorage.getItem("assignedProfessor");

	if (studentsAssigned) {
		const toasBody = document.getElementById("toast-content");
		toasBody.innerHTML = "";
		const content = document.createTextNode("Alumnos asignados correctamente");
		toasBody.appendChild(content);
		toast.show();

		localStorage.removeItem("assignedStudents");
	}

	if (professorAssigned) {
		const toasBody = document.getElementById("toast-content");
		toasBody.innerHTML = "";
		const content = document.createTextNode("Profesor asignado correctamente");
		toasBody.appendChild(content);
		toast.show();

		localStorage.removeItem("assignedProfessor");
	}
};

const successToast = document.getElementById("toast-success");
const toast = bootstrap.Toast.getOrCreateInstance(successToast);
const urlParams = new URLSearchParams(window.location.search);
const courseId = urlParams.get("courseId"); // ProfessorAsignment.js
const studentList = document.getElementById("student-list");
const modalAassignStudents = new mdb.Modal(
	document.getElementById("modal-add-student")
);

const submitStudents = document.getElementById("button-submit-students");

submitStudents.addEventListener("click", function () {
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
			window.location.href = "/Index";
		},
	});
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
