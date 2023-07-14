const regex = /^\s*.*\S.*\s*$/;
const inputProfessorSearch = document.getElementById("input-professor-search");
let assignedMembers = [];
const inputMemberRol = document.getElementById("input-member-rol");
let rolMember = "Integrante";
const urlParams = new URLSearchParams(window.location.search);
const academicBodyId = urlParams.get("academicBodyId");

inputMemberRol.addEventListener("change", function (event) {
	rolMember = event.target.value;
});

const connectionProfessor = new signalR.HubConnectionBuilder()
	.withUrl("/memberSearchHub")
	.build();

connectionProfessor
	.start()
	.then()
	.catch(function (err) {
		return console.error(err.toString());
		// window.location.reload();
	});

inputProfessorSearch.addEventListener("keyup", function (event) {
	const search = event.target.value;
	let isValid = regex.test(search);

	if (isValid) {
		connectionProfessor.invoke("SearchMember", search).catch(function (err) {
			return console.error(err.toString());
		});
	}

	event.preventDefault();
});

connectionProfessor.on("ReceiveProfessor", function (professors) {
	const professorList = document.getElementById("professor-list");
	const selectedProfessor = document.getElementById("container-professors");
	professorList.innerHTML = "";

	if (professors.length === 0) {
		const pofessorItem = document.createElement("li");
		pofessorItem.classList.add(
			"list-group-item",
			"border-0",
			"px-3",
			"py-2",
			"model-item"
		);
		const icon = document.createElement("i");
		icon.classList.add("fas", "fa-exclamation-triangle", "me-2");
		const text = document.createTextNode(
			"No se encontraron profesores registrados con esa búsqueda o ya se encuentra asignado a un Cuerpo Académico."
		);
		pofessorItem.appendChild(icon);
		pofessorItem.appendChild(text);
		professorList.appendChild(pofessorItem);
	}

	professors.forEach((professor) => {
		let professorItem = document.createElement("li");
		let nameSpan = document.createElement("span");
		let emailSpan = document.createElement("span");
		nameSpan.textContent = professor.fullName;
		emailSpan.textContent = professor.email;
		professorItem.appendChild(nameSpan);
		professorItem.appendChild(document.createElement("br"));
		professorItem.appendChild(emailSpan);
		professorItem.appendChild(document.createElement("br"));
		professorList.appendChild(professorItem);
		professorItem.style.borderRadius = "0";
		professorItem.classList.add(
			"list-group-item",
			"border-0",
			"px-3",
			"py-2",
			"model-item"
		);

		let isMemberInclude =
			assignedMembers.filter(
				(member) => member.professorId === professor.professorId
			).length > 0;

		if (isMemberInclude) {
			professorItem.classList.add("active");
		}

		professorItem.addEventListener("click", function () {
			isMemberInclude =
				assignedMembers.filter(
					(member) => member.professorId === professor.professorId
				).length > 0;

			if (isMemberInclude) {
				const toastBody = document.getElementById("toast-content");
				toastBody.innerHTML = "";
				const content = document.createTextNode(
					"El profesor ya se encuentra asignado."
				);
				toastBody.appendChild(content);
				toast.show();

				return;
			}

			let member = {
				academicBodyId: academicBodyId,
				professorId: professor.professorId,
				role: rolMember,
			};

			assignedMembers.push(member);
			// console.log(assignedMembers);

			const span = document.createElement("span");
			span.classList.add(
				"badge",
				"rounded-pill",
				"p-2",
				"mb-3",
				"mt-0",
				"px-3"
			);

			professorItem.classList.add("active");
			const spanName = document.createTextNode(professor.fullName);

			span.appendChild(spanName);
			span.appendChild(document.createElement("br"));

			if (rolMember === "Integrante") {
				span.classList.add("badge-primary");
				const spanRol = document.createTextNode("Integrante");
				span.appendChild(spanRol);
			} else {
				span.classList.add("badge-danger");
				const spanRol = document.createTextNode("Colaborador");
				span.appendChild(spanRol);
			}

			const deleteButton = document.createElement("i");
			deleteButton.classList.add("fas", "fa-times", "ms-2", "btn-link");
			deleteButton.setAttribute("role", "button");
			deleteButton.setAttribute("aria-label", "Eliminar filtro");
			span.appendChild(deleteButton);

			selectedProfessor.appendChild(span);
			deleteButton.addEventListener("click", function () {
				const index = assignedMembers
					.map(function (array) {
						return array.professorId;
					})
					.indexOf(professor.professorId);

				if (index !== -1) {
					assignedMembers.splice(index, 1);
				}

				professorItem.classList.remove("active");
				selectedProfessor.removeChild(span);
			});
		});
	});
});

const buttonCloseProfessors = document.getElementById(
	"button-close-professors"
);

buttonCloseProfessors.addEventListener("click", function () {
	assignedMembers = [];

	const professorList = document.getElementById("professor-list");
	const selectedProfessor = document.getElementById("container-professors");

	professorList.innerHTML = "";
	inputProfessorSearch.value = "";
	selectedProfessor.innerHTML = "";
});
