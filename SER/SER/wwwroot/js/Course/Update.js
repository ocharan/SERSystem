const errorToast = document.getElementById("toast-error");
const successToast = document.getElementById("toast-success");

if (errorToast) {
	const toast = bootstrap.Toast.getOrCreateInstance(errorToast);
	toast.show();
}

if (successToast) {
	const toast = bootstrap.Toast.getOrCreateInstance(successToast);
	toast.show();
}
