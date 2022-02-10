using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShoppingProject.DataAccess.Migrations
{
    public partial class AddStoreProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE Proc_GetCoverTypes
                                   As
                                   Select * from GetCoverTypes
                                ");
            migrationBuilder.Sql(@"CREATE PROCEDURE Proc_GetCoverType
                                   @Id int
                                   As
                                   Select * from GetCoverTypes where Id=@Id
                                ");
            migrationBuilder.Sql(@"CREATE PROCEDURE Proc_CreateCoverType
                                  @Name Varchar(50) 
                                  As
                                  Insert GetCoverTypes Values(@Name)
                                ");
            migrationBuilder.Sql(@"CREATE PROCEDURE Proc_UpdateCoverType
                                   @Id int,
                                   @Name Varchar(50)
                                   As
                                   Update GetCoverTypes set Name=@Name where Id=@Id
                                ");
            migrationBuilder.Sql(@"CREATE PROCEDURE Proc_DeleteCoverType
                                   @Id int
                                   As
                                   Delete from GetCoverTypes where Id=@Id
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Drop PROCEDURE Proc_DeleteCoverType");
            migrationBuilder.Sql(@"Drop PROCEDURE Proc_UpdateCoverType");
            migrationBuilder.Sql(@"Drop PROCEDURE Proc_CreateCoverType");
            migrationBuilder.Sql(@"Drop PROCEDURE Proc_GetCoverType");
            migrationBuilder.Sql(@"Drop PROCEDURE Proc_GetCoverTypes");
        }
    }
}
