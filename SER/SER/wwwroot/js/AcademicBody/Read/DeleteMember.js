const removeMemberButtons = document.getElementsByClassName(
	"button-remove-member"
);

const modalRemoveMember = new mdb.Modal(
	document.getElementById("modal-remove-member")
);

const removeMemberName = document.getElementById("span-member-name");
let memberId;

for (let i = 0; i < removeMemberButtons.length; i++) {
	removeMemberButtons[i].addEventListener("click", function () {
		memberId = removeMemberButtons[i].getAttribute("data-member-id");
		removeMemberName.textContent = `● ${removeMemberButtons[i].getAttribute(
			"data-member-name"
		)}`;
	});
}

const confirmRemoveMember = document.getElementById(
	"button-modal-remove-member"
);

confirmRemoveMember.addEventListener("click", function () {
	modalRemoveMember.hide();

	$.ajax({
		type: "POST",
		url: "?handler=DeleteMember",
		contentType: "application/json",
		data: JSON.stringify(memberId),
		headers: {
			RequestVerificationToken: $(
				'input:hidden[name="__RequestVerificationToken"]'
			).val(),
		},
		success: function () {
			localStorage.setItem("removedMember", "true");
			location.reload();
		},
		error: function () {
			const toastBody = document.getElementById("toast-error-content");
			toastBody.innerHTML = "";
			const content = document.createTextNode(
				"Ha ocurrido un error al retirar al miembro del CA."
			);
			toastBody.appendChild(content);
			toastError.show();
			toast.hide();
		},
	});
});
