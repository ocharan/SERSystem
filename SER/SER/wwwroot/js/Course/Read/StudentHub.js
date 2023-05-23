// const regex = /^\S.*\S$/;
const regex = /^\s*.*\S.*\s*$/;
const inputStudentSearch = document.getElementById("input-student-search");
let assignedStudents = []; // CourseRegistrations.js

const connectionStudent = new signalR.HubConnectionBuilder()
	.withUrl("/studentSearchHub")
	.build();

connectionStudent
	.start()
	.then()
	.catch(function (err) {
		return console.error(err.toString());
		// window.location.reload();
	});

inputStudentSearch.addEventListener("keyup", function (event) {
	const search = event.target.value;
	let isValid = regex.test(search);

	if (isValid) {
		connectionStudent.invoke("SearchStudent", search).catch(function (err) {
			return console.error(err.toString());
		});
	}

	event.preventDefault();
});

connectionStudent.on("ReceiveStudents", function (students) {
	const selectedStudents = document.getElementById("container-students");
	studentList.innerHTML = "";

	students.forEach((student) => {
		let studentItem = document.createElement("li");
		let nameSpan = document.createElement("span");
		let emailSpan = document.createElement("span");
		nameSpan.textContent = student.fullName;
		emailSpan.textContent = `${student.email} | ${student.enrollment}`;
		studentItem.appendChild(nameSpan);
		studentItem.appendChild(document.createElement("br"));
		studentItem.appendChild(emailSpan);
		studentItem.appendChild(document.createElement("br"));
		studentList.appendChild(studentItem);
		studentItem.style.borderRadius = "0";
		studentItem.classList.add(
			"list-group-item",
			"border-0",
			"px-3",
			"py-2",
			"rounded-0",
			"model-item"
		);

		if (assignedStudents.includes(student.studentId)) {
			studentItem.classList.add("active");
		}

		studentItem.addEventListener("click", function () {
			if (assignedStudents.includes(student.studentId)) {
				const toasBody = document.getElementById("toast-content");
				toasBody.innerHTML = "";
				const content = document.createTextNode("El alumno ya está asignado");
				toasBody.appendChild(content);
				toast.show();

				return;
			}

			studentItem.classList.add("active");
			assignedStudents.push(student.studentId);

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
			const spanText = document.createTextNode(student.fullName);
			span.appendChild(spanText);
			const deleteButton = document.createElement("i");
			deleteButton.classList.add("fas", "fa-times", "ms-2", "btn-link");
			deleteButton.setAttribute("role", "button");
			deleteButton.setAttribute("aria-label", "Eliminar filtro");
			span.appendChild(deleteButton);
			selectedStudents.appendChild(span);

			deleteButton.addEventListener("click", function () {
				const index = assignedStudents.indexOf(student.studentId);
				if (index !== -1) {
					assignedStudents.splice(index, 1);
				}

				selectedStudents.removeChild(span);

				studentItem.classList.remove("active");
			});
		});
	});

	if (students.length === 0) {
		const studentItem = document.createElement("li");
		const icon = document.createElement("i");
		icon.classList.add("fas", "fa-exclamation-triangle", "me-2");
		const text = document.createTextNode(
			"El alumno no se encuentra registrado o ya está asignado a un curso que se encuentra abierto. "
		);
		studentItem.appendChild(icon);
		studentItem.appendChild(text);
		studentItem.classList.add(
			"list-group-item",
			"border-0",
			"px-3",
			"py-2",
			"model-item"
		);
		studentList.appendChild(studentItem);
	}
});
