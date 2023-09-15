const submitMembers = document.getElementById("button-submit-members");

submitMembers.addEventListener("click", function () {
	if (assignedMembers.length == 0) {
		const toastBody = document.getElementById("toast-content");
		toastBody.innerHTML = "";
		const content = document.createTextNode(
			"No ha seleccionado a ningún miembro"
		);
		toastBody.appendChild(content);
		toast.show();
	} else {
		const jsonContent = JSON.stringify(assignedMembers);

		// console.log(jsonContent);

		$.ajax({
			type: "POST",
			url: "?handler=CreateMembers",
			contentType: "application/json",
			data: jsonContent,
			headers: {
				RequestVerificationToken: $(
					'input:hidden[name="__RequestVerificationToken"]'
				).val(),
			},
			success: function () {
				location.reload();
				localStorage.setItem("assignedMembers", "true");
			},
			error: function () {
				const toastBody = document.getElementById("toast-error-content");
				toastBody.innerHTML = "";
				const content = document.createTextNode(
					"Ha ocurrido un error al asignar los miembros"
				);
				toastBody.appendChild(content);
				toastError.show();
				toast.hide();
			},
		});
	}
});