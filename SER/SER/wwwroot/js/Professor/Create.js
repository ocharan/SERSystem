const professorEmail = document.getElementById("input-email");
const professorUsername = document.getElementById("input-username");

professorEmail.oninput = function (event) {
	let email = event.target.value;

	if (email.includes("@")) {
		let username = email.substring(0, email.indexOf("@"));
		professorUsername.value = username;
	} else {
		professorUsername.value = email;
	}
};
