const filters = document.getElementsByClassName("dropdown-item-filter");
const filterContainer = document.getElementById("selected-filter-container");

for (let i = 0; i < filters.length; i++) {
	filters[i].addEventListener("click", function () {
		filterContainer.innerHTML = "";
		let selectedFilter = filters[i].innerText;
		localStorage.setItem("SelectedFilter", selectedFilter);
	});
}

const buttonReloadTable = document.getElementById("button-reload-table");

buttonReloadTable.addEventListener("click", function () {
	localStorage.removeItem("SelectedFilter");

	if (filterContainer.classList.contains("btn-link")) {
		filterContainer.parentElement.parentElement.remove();
	}
});

filterContainer.addEventListener("click", function (event) {
	if (event.target.classList.contains("btn-link")) {
		event.target.parentElement.parentElement.remove();
		localStorage.removeItem("SelectedFilter");

		let url = new URL(window.location.href);
		let searchParams = new URLSearchParams(url.search);
		searchParams.delete("currentFilter");
		let newUrl = url.pathname + "?" + searchParams.toString();
		window.location.href = newUrl;
	}
});

const successToast = document.getElementById("toast-success");

if (successToast) {
	const toast = bootstrap.Toast.getOrCreateInstance(successToast);
	toast.show();
}

const errorToast = document.getElementById("toast-error");

if (errorToast) {
	const toast = bootstrap.Toast.getOrCreateInstance(errorToast);
	toast.show();
}

const emails = document.querySelectorAll(".text-email");

for (let i = 0; i < emails.length; i++) {
	emails[i].addEventListener("click", function () {
		navigator.clipboard.writeText(emails[i].innerText).then(() => {
			const copyToast = document.getElementById("toast-copy");

			if (copyToast) {
				const toast = bootstrap.Toast.getOrCreateInstance(copyToast);
				toast.show();
			}
		});
	});
}

window.onload = function () {
	let selectedFilter = localStorage.getItem("SelectedFilter");

	let url = new URL(window.location.href);
	let searchParams = new URLSearchParams(url.search);
	let hasFilter = searchParams.has("currentFilter");

	if (selectedFilter && hasFilter) {
		let spanBadge = `
      <div class="col">
        <span class="badge badge-primary mb-3">
          <i class="fas fa-times me-2 btn-link" role="button" aria-label="Eliminar filtro"></i>
          ${selectedFilter}
        </span>
      </div>        
    `;

		filterContainer.innerHTML += spanBadge;
	} else {
		localStorage.removeItem("SelectedFilter");
	}
};
