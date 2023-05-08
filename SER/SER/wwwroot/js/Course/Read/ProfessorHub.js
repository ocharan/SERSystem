// const regex = /^\S.*\S$/;
const inputProfessorSearch = document.getElementById("input-professor-search");

const connectionProfessor = new signalR.HubConnectionBuilder()
	.withUrl("/professorSearchHub")
	.build();

connectionProfessor
	.start()
	.then()
	.catch(function (err) {
		return console.error(err.toString());
	});

inputProfessorSearch.addEventListener("keyup", function (event) {
	const search = event.target.value;
	let isValid = regex.test(search);

	if (isValid) {
		connectionProfessor.invoke("SearchProfessor", search).catch(function (err) {
			return console.error(err.toString());
		});
	}

	event.preventDefault();
});

connectionProfessor.on("ReceiveProfessor", function (professors) {
	const professorList = document.getElementById("professor-list");
	const selectedProfessor = document.getElementById(
		"container-selecteed-professor"
	);
	professorList.innerHTML = "";

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
		professorItem.classList.add("list-group-item", "border-0", "px-3", "py-2");

		professorItem.addEventListener("click", function () {
      assignedProfessor = professor;
			selectedProfessor.innerHTML = "";
			const span = document.createElement("span");
			span.classList.add(
				"badge",
				"badge-primary",
				"rounded-pill",
				"p-2",
				"mb-3",
				"mt-0",
				"px-3"
			);
			const spanText = document.createTextNode(professor.fullName);
			span.appendChild(spanText);
			const deleteButton = document.createElement("i");
			deleteButton.classList.add("fas", "fa-times", "ms-2", "btn-link");
			deleteButton.setAttribute("role", "button");
			deleteButton.setAttribute("aria-label", "Eliminar filtro");
			span.appendChild(deleteButton);
			selectedProfessor.appendChild(span);

			deleteButton.addEventListener("click", function () {
				selectedProfessor.innerHTML = "";
			});
		});
	});
});
