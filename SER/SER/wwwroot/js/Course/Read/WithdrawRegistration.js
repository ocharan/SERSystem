const confirmRemoveStudent = document.getElementById(
	"button-modal-remove-student"
);
const removeStudentsButtons = document.getElementsByClassName(
	"button-remove-student"
);
const modalRemoveStudent = new mdb.Modal(
	document.getElementById("modal-remove-student")
);
const removeStudentName = document.getElementById("span-student-name");
let registrationId;

for (let i = 0; i < removeStudentsButtons.length; i++) {
	removeStudentsButtons[i].addEventListener("click", function () {
		registrationId = removeStudentsButtons[i].getAttribute("data-id");
		removeStudentName.textContent = `● ${removeStudentsButtons[i].getAttribute(
			"data-name"
		)}`;
	});
}

confirmRemoveStudent.addEventListener("click", function () {
	modalRemoveStudent.hide();

	$.ajax({
		type: "POST",
		url: "?handler=WithdrawCourseRegistration",
		contentType: "application/json",
		data: JSON.stringify(registrationId),
		headers: {
			RequestVerificationToken: $(
				'input:hidden[name="__RequestVerificationToken"]'
			).val(),
		},
		success: function () {
			location.reload();
			localStorage.setItem("removedStudent", "true");
		},
		error: function () {
			const toastBody = document.getElementById("toast-error-content");
			toastBody.innerHTML = "";
			const content = document.createTextNode(
				"Ha ocurrido un error al retirar al estudiante del curso."
			);
			toastBody.appendChild(content);
			toastError.show();
			toast.hide();
		},
	});
});
