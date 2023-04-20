const formAssingToken = document.getElementById("form-assign-token");
const modalSendEmail = document.getElementById("modal-send-email");
const modal = new mdb.Modal(modalSendEmail);

formAssingToken.addEventListener("submit", () => {
	const inputUsername = document.getElementById("input-username");

	if (inputUsername.checkValidity()) {
		modal.show();
	}
});
