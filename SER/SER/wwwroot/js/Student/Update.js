const formUpdateStudent = document.getElementById("form-update-student");
const modalUpdateStudent = document.getElementById("modal-update-student");
const modal = new mdb.Modal(modalUpdateStudent);

formUpdateStudent.addEventListener("submit", () => {
	let isFullNameValid = document
		.getElementById("input-fullname")
		.checkValidity();
	let isEmailValid = document.getElementById("input-email").checkValidity();

	if (isFullNameValid && isEmailValid) {
		modal.show();
	}
});
