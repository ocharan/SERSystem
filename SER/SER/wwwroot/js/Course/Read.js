"use strict";

// window.onload = function () {
// 	let studentsAssigned = localStorage.getItem("assignedStudents");

// 	if (studentsAssigned) {
// 		const toasBody = document.getElementById("toast-content");
// 		toasBody.innerHTML = "";
// 		const content = document.createTextNode("Alumnos asignados correctamente");
// 		toasBody.appendChild(content);
// 		toast.show();

// 		localStorage.removeItem("assignedStudents");
// 	}
// };

// const regex = /^\S.*\S$/;
// let assignedStudents = [];
// const successToast = document.getElementById("toast-success");
// const toast = bootstrap.Toast.getOrCreateInstance(successToast);
// const urlParams = new URLSearchParams(window.location.search);
// const courseId = urlParams.get("courseId");
// const inputStudentSearch = document.getElementById("input-student-search");
// const inputProfessorSearch = document.getElementById("input-professor-search");
// const studentList = document.getElementById("student-list");
// const modalAassignStudents = new mdb.Modal(
// 	document.getElementById("modal-add-student")
// );

// const connectionProfessor = new signalR.HubConnectionBuilder()
// 	.withUrl("/professorSearchHub")
// 	.build();

// connectionProfessor
// 	.start()
// 	.then()
// 	.catch(function (err) {
// 		return console.error(err.toString());
// 	});

// inputProfessorSearch.addEventListener("keyup", function (event) {
// 	const search = event.target.value;
// 	let isValid = regex.test(search);

// 	if (isValid) {
// 		connectionProfessor.invoke("SearchProfessor", search).catch(function (err) {
// 			return console.error(err.toString());
// 		});
// 	}

// 	event.preventDefault();
// });

// connectionProfessor.on("ReceiveProfessor", function (professors) {
// 	const professorList = document.getElementById("professor-list");
// 	const selectedProfessor = document.getElementById(
// 		"container-selecteed-professor"
// 	);
// 	professorList.innerHTML = "";

// 	professors.forEach((professor) => {
// 		let professorItem = document.createElement("li");
// 		let nameSpan = document.createElement("span");
// 		let emailSpan = document.createElement("span");
// 		nameSpan.textContent = professor.fullName;
// 		emailSpan.textContent = professor.email;
// 		professorItem.appendChild(nameSpan);
// 		professorItem.appendChild(document.createElement("br"));
// 		professorItem.appendChild(emailSpan);
// 		professorItem.appendChild(document.createElement("br"));
// 		professorList.appendChild(professorItem);
// 		professorItem.classList.add("list-group-item", "border-0", "px-3", "py-2");

// 		professorItem.addEventListener("click", function () {
// 			selectedProfessor.innerHTML = "";
// 			const span = document.createElement("span");
// 			span.classList.add(
// 				"badge",
// 				"badge-primary",
// 				"rounded-pill",
// 				"p-2",
// 				"mb-3",
// 				"mt-0",
// 				"px-3"
// 			);
// 			const spanText = document.createTextNode(professor.fullName);
// 			span.appendChild(spanText);
// 			const deleteButton = document.createElement("i");
// 			deleteButton.classList.add("fas", "fa-times", "ms-2", "btn-link");
// 			deleteButton.setAttribute("role", "button");
// 			deleteButton.setAttribute("aria-label", "Eliminar filtro");
// 			span.appendChild(deleteButton);
// 			selectedProfessor.appendChild(span);

// 			deleteButton.addEventListener("click", function () {
// 				selectedProfessor.innerHTML = "";
// 			});
// 		});
// 	});
// });

// const connectionStudent = new signalR.HubConnectionBuilder()
// 	.withUrl("/studentSearchHub")
// 	.build();

// connectionStudent
// 	.start()
// 	.then()
// 	.catch(function (err) {
// 		return console.error(err.toString());
// 	});

// inputStudentSearch.addEventListener("keyup", function (event) {
// 	const search = event.target.value;
// 	let isValid = regex.test(search);

// 	if (isValid) {
// 		connectionStudent.invoke("SearchStudent", search).catch(function (err) {
// 			return console.error(err.toString());
// 		});
// 	}

// 	event.preventDefault();
// });

// connectionStudent.on("ReceiveStudents", function (students) {
// 	const selectedStudents = document.getElementById("container-students");
// 	studentList.innerHTML = "";

// 	students.forEach((student) => {
// 		let studentItem = document.createElement("li");
// 		let nameSpan = document.createElement("span");
// 		let emailSpan = document.createElement("span");
// 		nameSpan.textContent = student.fullName;
// 		emailSpan.textContent = `${student.email} | ${student.enrollment}`;
// 		studentItem.appendChild(nameSpan);
// 		studentItem.appendChild(document.createElement("br"));
// 		studentItem.appendChild(emailSpan);
// 		studentItem.appendChild(document.createElement("br"));
// 		studentList.appendChild(studentItem);
// 		studentItem.classList.add("list-group-item", "border-0", "px-3", "py-2");

// 		studentItem.addEventListener("click", function () {
// 			if (assignedStudents.includes(student.studentId)) {
// 				const toasBody = document.getElementById("toast-content");
// 				toasBody.innerHTML = "";
// 				const content = document.createTextNode("El alumno ya está asignado");
// 				toasBody.appendChild(content);
// 				toast.show();

// 				return;
// 			}

// 			assignedStudents.push(student.studentId);

// 			const span = document.createElement("span");
// 			span.classList.add(
// 				"badge",
// 				"badge-primary",
// 				"rounded-pill",
// 				"p-2",
// 				"mb-3",
// 				"mt-0",
// 				"px-3"
// 			);
// 			const spanText = document.createTextNode(student.fullName);
// 			span.appendChild(spanText);
// 			const deleteButton = document.createElement("i");
// 			deleteButton.classList.add("fas", "fa-times", "ms-2", "btn-link");
// 			deleteButton.setAttribute("role", "button");
// 			deleteButton.setAttribute("aria-label", "Eliminar filtro");
// 			span.appendChild(deleteButton);
// 			selectedStudents.appendChild(span);

// 			deleteButton.addEventListener("click", function () {
// 				const index = assignedStudents.indexOf(student.studentId);
// 				if (index !== -1) {
// 					assignedStudents.splice(index, 1);
// 				}

// 				selectedStudents.removeChild(span);
// 			});
// 		});
// 	});

// 	if (students.length === 0) {
// 		const studentItem = document.createElement("li");
// 		const icon = document.createElement("i");
// 		icon.classList.add("fas", "fa-exclamation-triangle", "me-2");
// 		const text = document.createTextNode(
// 			"El alumno no se encuentra registrado o ya está asignado a un curso que se encuentra vigente. "
// 		);
// 		studentItem.appendChild(icon);
// 		studentItem.appendChild(text);
// 		studentItem.classList.add("list-group-item", "border-0", "px-3", "py-2");
// 		studentList.appendChild(studentItem);
// 	}
// });

// const submitStudents = document.getElementById("button-submit-students");

// submitStudents.addEventListener("click", function () {
// 	const courseRegistrations = assignedStudents.map((studentId) =>
// 		createCourseRegistration(studentId)
// 	);

// 	const jsonContent = JSON.stringify(courseRegistrations);

// 	$.ajax({
// 		type: "POST",
// 		url: "?handler=CreateCourseRegistrations",
// 		contentType: "application/json",
// 		data: jsonContent,
// 		headers: {
// 			RequestVerificationToken: $(
// 				'input:hidden[name="__RequestVerificationToken"]'
// 			).val(),
// 		},
// 		success: function (data) {
// 			location.reload();
// 			localStorage.setItem("assignedStudents", "true");
// 		},
// 		error: function () {
// 			window.location.href = "/Index";
// 		},
// 	});
// });

// function createCourseRegistration(studentId) {
// 	let courseRegistration = {
// 		CourseId: courseId,
// 		StudentId: studentId,
// 	};

// 	return courseRegistration;
// }

// const buttonCloseStudents = document.getElementById("button-close-students");

// buttonCloseStudents.addEventListener("click", function () {
// 	clearList();
// });

// function clearList() {
// 	assignedStudents = [];
// 	const selectedStudents = document.getElementById("container-students");
// 	selectedStudents.innerHTML = "";
// 	inputStudentSearch.value = "";
// 	studentList.innerHTML = "";
// }
