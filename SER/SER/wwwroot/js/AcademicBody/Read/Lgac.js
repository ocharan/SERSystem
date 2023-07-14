// Success Toasts
window.onload = function () {
	let assignedLgac = localStorage.getItem("assignedLgac");
	let updateLgac = localStorage.getItem("updateLgac");
	let deleteLgac = localStorage.getItem("deleteLgac");

	if (assignedLgac) {
		showToast("LGAC creada y asignada correctamente", "assignedLgac");
	}

	if (updateLgac) {
		showToast("LGAC actualizada correctamente", "updateLgac");
	}

	if (deleteLgac) {
		showToast("LGAC eliminada correctamente", "deleteLgac");
	}
};

const successToast = document.getElementById("toast-success");
const errorToast = document.getElementById("toast-error");
const toast = bootstrap.Toast.getOrCreateInstance(successToast);
const toastError = bootstrap.Toast.getOrCreateInstance(errorToast);

function showToast(message, localStorageItem) {
	const toasBody = document.getElementById("toast-content");
	toasBody.innerHTML = "";
	const content = document.createTextNode(message);
	toasBody.appendChild(content);
	toast.show();

	localStorage.removeItem(localStorageItem);
}

// Create LGAC
const lgacForm = document.getElementById("form-create-lgac");

lgacForm.addEventListener("submit", function (event) {
	event.preventDefault();

	let formData = {
		Name: document.getElementById("input-lgac-name").value,
		Description: document.getElementById("input-lgac-description").value,
		AcademicBodyId: document.getElementById("input-lgac-adademicbody-id").value,
	};

	const jsonContent = JSON.stringify(formData);

	$.ajax({
		type: "POST",
		url: "?handler=CreateLgac",
		contentType: "application/json",
		data: jsonContent,
		headers: {
			RequestVerificationToken: $(
				'input:hidden[name="__RequestVerificationToken"]'
			).val(),
		},
		success: function () {
			location.reload();
			localStorage.setItem("assignedLgac", "true");
		},
		error: function () {
			const toasBody = document.getElementById("toast-error-content");
			toasBody.innerHTML = "";
			const content = document.createTextNode(
				"Ha ocurrido un error al asignar y crear la LGAC al CA."
			);
			toasBody.appendChild(content);
			toastError.show();
			toast.hide();
		},
	});
});

// Update LGAC
const modalUpdateLgac = new mdb.Modal(
	document.getElementById("modal-update-lgac")
);

const udpateLgacs = document.getElementsByClassName("button-update-lgac");

for (let i = 0; i < udpateLgacs.length; i++) {
	udpateLgacs[i].addEventListener("click", function () {
		modalUpdateLgac.show();

		const lgacName = this.getAttribute("data-lgac-name");
		const lgacDescription = this.getAttribute("data-lgac-description");
		const lgacId = this.getAttribute("data-lgac-id");

		document.getElementById("input-lgac-update-id").value = lgacId;
		document.getElementById("input-lgac-update-name").value = lgacName;
		document.getElementById("input-lgac-update-description").value =
			lgacDescription;
	});
}

const lgacFormUpdate = document.getElementById("form-update-lgac");

lgacFormUpdate.addEventListener("submit", function (event) {
	event.preventDefault();

	let formData = {
		LgacId: document.getElementById("input-lgac-update-id").value,
		Name: document.getElementById("input-lgac-update-name").value,
		Description: document.getElementById("input-lgac-update-description").value,
		AcademicBodyId: document.getElementById("input-academic-body-update-id")
			.value,
	};

	const jsonContent = JSON.stringify(formData);

	$.ajax({
		type: "POST",
		url: "?handler=UpdateLgac",
		contentType: "application/json",
		data: jsonContent,
		headers: {
			RequestVerificationToken: $(
				'input:hidden[name="__RequestVerificationToken"]'
			).val(),
		},
		success: function () {
			location.reload();
			localStorage.setItem("updateLgac", "true");
		},
		error: function () {
			const toasBody = document.getElementById("toast-error-content");
			toasBody.innerHTML = "";
			const content = document.createTextNode(
				"Ha ocurrido un error al actualizar la LGAC."
			);
			toasBody.appendChild(content);
			toastError.show();
			toast.hide();
		},
	});
});

// Delete LGAC
const modalDeleteLgac = new mdb.Modal(
	document.getElementById("modal-delete-lgac")
);
const deleteLgacs = document.getElementsByClassName("button-delete-lgac");
const removeNameLgac = document.getElementById("remove-name-lgac");

for (let i = 0; i < deleteLgacs.length; i++) {
	deleteLgacs[i].addEventListener("click", function () {
		removeNameLgac.textContent = `● ${this.getAttribute("data-lgac-name")}`;
		const lgacId = this.getAttribute("data-lgac-id");
		document.getElementById("input-lgac-delete-id").value = lgacId;
		modalDeleteLgac.show();
	});
}

const lgacFormDelete = document.getElementById("form-delete-lgac");

lgacFormDelete.addEventListener("submit", function (event) {
	event.preventDefault();

	let formData = {
		LgacId: document.getElementById("input-lgac-delete-id").value,
	};

	const jsonContent = JSON.stringify(formData);

	$.ajax({
		type: "POST",
		url: "?handler=DeleteLgac",
		contentType: "application/json",
		data: jsonContent,
		headers: {
			RequestVerificationToken: $(
				'input:hidden[name="__RequestVerificationToken"]'
			).val(),
		},
		success: function () {
			location.reload();
			localStorage.setItem("deleteLgac", "true");
		},
		error: function () {
			const toasBody = document.getElementById("toast-error-content");
			toasBody.innerHTML = "";
			const content = document.createTextNode(
				"Ha ocurrido un error al eliminar la LGAC."
			);
			toasBody.appendChild(content);
			toastError.show();
			toast.hide();
		},
	});
});
