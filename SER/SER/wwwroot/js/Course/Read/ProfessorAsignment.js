// window.onload = function () {
//   let studentsAssigned = localStorage.getItem("assignedStudents");
//   let professorAssigned = localStorage.getItem("assignedProfessor");

//   if (studentsAssigned) {
//     const toasBody = document.getElementById("toast-content");
//     toasBody.innerHTML = "";
//     const content = document.createTextNode("Alumnos asignados correctamente");
//     toasBody.appendChild(content);
//     toast.show();

//     localStorage.removeItem("assignedStudents");
//   }

//   if (professorAssigned) {
//     const toasBody = document.getElementById("toast-content");
//     toasBody.innerHTML = "";
//     const content = document.createTextNode("Profesor asignado correctamente");
//     toasBody.appendChild(content);
//     toast.show();

//     localStorage.removeItem("assignedProfessor");
//   }
// }

let assignedProfessor = {};
const submitProfessor = document.getElementById("button-submit-professor");

submitProfessor.addEventListener("click", function () {
	const body = {
		CourseId: Number(courseId),
		ProfessorId: assignedProfessor.professorId,
	};

	console.log(body);

	const jsonContent = JSON.stringify(body);

	console.log(jsonContent);

	$.ajax({
		type: "POST",
		url: "?handler=CreateProfessorAssignment",
		contentType: "application/json",
		data: jsonContent,
		headers: {
			RequestVerificationToken: $(
				'input:hidden[name="__RequestVerificationToken"]'
			).val(),
		},
		success: function () {
			location.reload();
			localStorage.setItem("assignedProfessor", "true");
		},
	});
});

const buttonCloseProfessor = document.getElementById("button-close-professor");

buttonCloseProfessor.addEventListener("click", function () {
	clearProfessor();
});

function clearProfessor() {
	assignedProfessor = {};
	const selectedProfessor = document.getElementById(
		"container-selecteed-professor"
	);
	selectedProfessor.innerHTML = "";
}
