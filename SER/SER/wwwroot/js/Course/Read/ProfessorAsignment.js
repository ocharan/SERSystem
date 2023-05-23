let assignedProfessor = {};
const submitProfessor = document.getElementById("button-submit-professor");

submitProfessor.addEventListener("click", function () {
	const body = {
		CourseId: Number(courseId),
		ProfessorId: assignedProfessor.professorId,
	};

	const jsonContent = JSON.stringify(body);

	if (assignedProfessor.professorId != undefined) {
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
				localStorage.setItem("assignedProfessor", "true");
				location.reload();
			},
			error: function () {
				const toasBody = document.getElementById("toast-error-content");
				toasBody.innerHTML = "";
				const content = document.createTextNode(
					"Ha ocurrido un error al asignar el profesor al curso."
				);
				toasBody.appendChild(content);
				toastError.show();
				toast.hide();
			},
		});
	} else {
		const toasBody = document.getElementById("toast-content");
		toasBody.innerHTML = "";
		const content = document.createTextNode("No ha seleccionado a un profesor");
		toasBody.appendChild(content);
		toast.show();
	}
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
