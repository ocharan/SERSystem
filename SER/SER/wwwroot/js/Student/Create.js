const formCreateStudent = document.getElementById("form-create-student");
const modalCreateStudent = document.getElementById("modal-create-student");
const modal = new mdb.Modal(modalCreateStudent);

formCreateStudent.addEventListener("submit", () => {
	let isFullNameValid = document
		.getElementById("input-fullname")
		.checkValidity();
	let isEnrollmentValid = document
		.getElementById("input-enrollment")
		.checkValidity();
	let isEmailValid = document.getElementById("input-email").checkValidity();

	if (isFullNameValid && isEnrollmentValid && isEmailValid) {
		modal.show();
	}
});
