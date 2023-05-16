const buttonLogout = document.getElementById("button-log-out");
const nextButton = buttonLogout.nextElementSibling;
nextButton.style.display = "none";

const sidebarItems = document.querySelectorAll(".sidebar ul li");

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
		sidebarItems[index].classList.add("active");
		sidebarItems[index].style.borderRadius = "10px";
	}
});

const openButton = document.getElementById("open-button");
openButton.addEventListener("click", () => {
	const sidebar = document.querySelector(".sidebar");
	sidebar.classList.add("active");
});

const closeButton = document.getElementById("close-button");
closeButton.addEventListener("click", () => {
	const sidebar = document.querySelector(".sidebar");
	sidebar.classList.remove("active");
});
