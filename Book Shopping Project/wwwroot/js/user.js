var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width":"15%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id:"id",lockoutEnd:"lockoutEnd"
                },
                "render":function(data) {
                    var today = new Date().getTime();

                    var lockOut = new Date(data.lockoutEnd).getTime();

                    if (lockOut > today) {
                        return `

                       <div class="text-center">
                      <a onclick=LockUnLock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                        UnLock
                        </a>
                       </div>

                    `;
                    }
                    else
                    {
                        //User UnLock

                        return `

                       <div class="text-center">
                      <a onclick=LockUnLock('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                        Lock
                        </a>
                       </div>

                    `;

                    }

                }
            }
        ]
    })
}

function LockUnLock(id) {
   // alert('call');

    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.api().ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}