const URL = "http://localhost:8000/";

// A function to fetch our array of tasks (our data)
const getData = () => {
    fetch(URL)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            data.forEach(task => appendItem(task))
        });
}

const appendItem = (task) => {
    let container = document.getElementById("task-container");
    let div = document.createElement("div");
    let header = document.createElement("h3");
    let p = document.createElement("p");

    div.id = task.Id;
    div.className = "task";

    
    header.innerText = task.Title;
    p.innerText = task.Description;
    
    div.appendChild(header);
    div.appendChild(p);
    
    if (task.IsComplete) {
        div.classList.add("completed");
        moveTask(div);
    } else {
        div.onclick = () => completeTask(task.Id);
        container.appendChild(div);
    }
}

const handleSubmit = (event) => {
    // Stops page from reloading
    event.preventDefault();
    console.log(event);
    
    const formData = new FormData(event.target);
    //Convert form data into simple object
    const obj = Object.fromEntries(formData);
    console.log(obj);
    
    fetch(URL, {
        method: "POST",
        Mode: "cors",
        body: JSON.stringify(obj)
    })
    .then(response => {
            if (!response.ok) {
                throw new Error(response.body);
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
            appendItem(data);
        })
        .catch(err => console.error(err))
    event.target.reset();
}

const completeTask = (taskId) => {
    const div = document.getElementById(taskId);

    fetch(URL, {
      method: "PUT",
      mode: "cors",
      body: JSON.stringify({ taskId })  
    })

    .then(response => {
        if (response.ok) {
            div.classList.add("completed");
            moveTask(div);
        }
    })
}

const moveTask = (div) => {
    const newParent = document.getElementById("completed-tasks-container");
    newParent.appendChild(div);
}