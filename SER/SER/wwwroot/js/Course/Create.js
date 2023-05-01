const inputIsOpen = document.getElementById("input-is-open");
const textIsOpen = document.getElementById("text-is-open");
const inputIsOpenAuxiliary = document.getElementById("input-is-open-auxiliary");

window.onload = function () {
	inputIsOpenAuxiliary.value = !inputIsOpen.checked;
};

inputIsOpen.addEventListener("change", function () {
	if (inputIsOpen.checked) {
		textIsOpen.innerText = "Cerrado";
	} else {
		textIsOpen.innerText = "Abierto";
	}

	inputIsOpenAuxiliary.value = !inputIsOpen.checked;
});
