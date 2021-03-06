var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "20%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "isAuthorizedCompany","width":"5%",
                "render": function (data) {
                    if (data) {
                        return `
                                  <input type="checkbox"  disabled checked />
                                `;
                    }
                    else {
                        return `
                                <input type="checkbox" disabled />
                                `;
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                            <a class="btn btn-primary" href="/Admin/Company/Upsert/${data}">
                               <i class="fas fa-edit"></i>
                            </a>&nbsp;&nbsp;
                            <a class="btn btn-danger" onclick=Delete("/Admin/Company/Delete/${data}")>
                            <i class="fas fa-trash-alt"></i>
                            </a>

                            </div>
                            `;
                }
            }

        ]
    })
}

function Delete(url) {
    swal({
        title: "Want To Delete Data ?",
        icon: "warning",
        text: "Delete Information!!!",
        buttons: true,
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }

    })
}