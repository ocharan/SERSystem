const anchorNavbarOptions = document.getElementsByClassName(
	"anchor-navbar-option"
);

for (let i = 0; i < anchorNavbarOptions.length; i++) {
	anchorNavbarOptions[i].addEventListener("click", () => {
		for (let j = 0; j < anchorNavbarOptions.length; j++) {
			anchorNavbarOptions[j].classList.remove("active");
		}

		anchorNavbarOptions[i].classList.add("active");
	});
}

let currentUrl = window.location.href;
const namesNavbarOptions = [
	"Management",
	"Course",
	"Docentes",
	"Documentos",
	"Coordinadores",
	"Student",
	"Expedientes",
];

namesNavbarOptions.forEach((name, index) => {
	if (currentUrl.includes(name)) {
		anchorNavbarOptions[index].classList.add("active");
		anchorNavbarOptions[index].style.borderRadius = "10px";
	}
});

const buttonLogout = document.getElementById("button-log-out");
const nextButton = buttonLogout.nextElementSibling;
nextButton.style.display = "none";
