const errorToast = document.getElementById("toast-error");

if (errorToast) {
	const toast = bootstrap.Toast.getOrCreateInstance(errorToast);
	toast.show();
}
