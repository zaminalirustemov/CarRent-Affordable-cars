let deleteImageBtns = document.querySelectorAll(".delete-image-btn")

deleteImageBtns.forEach(btn => btn.addEventListener("click", function () {
    btn.parentElement.remove()
}))

let softDeleteBtns = document.querySelectorAll(".softdelete-btn")

softDeleteBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    Swal.fire({
        title: 'Are you sure?',
        text: "Are you sure you want to delete?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(Response => {
                    if (Response.status == 200) {
                        window.location.reload(true);
                    } else {
                        alert("No such data is available.")
                    }
                })
            
        }
    })
}))

let hardDeleteBtns = document.querySelectorAll(".harddelete-btn")

hardDeleteBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    Swal.fire({
        title: 'Are you sure?',
        text: "Are you sure you want to delete? Will not come back again.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(Response => {
                    if (Response.status == 200) {
                        window.location.reload(true);
                    } else {
                        alert("No such data is available.")
                    }
                })

        }
    })
}))

let allDeleteBtns = document.querySelectorAll(".all-delete-button")

allDeleteBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    Swal.fire({
        title: 'Are you sure?',
        text: "Are you sure you want to delete? Will not come back again.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url)
                .then(Response => {
                    if (Response.status == 200) {
                        window.location.reload(true);
                    } else {
                        alert("No such data is available.")
                    }
                })

        }
    })
}))