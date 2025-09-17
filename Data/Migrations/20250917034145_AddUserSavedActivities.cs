using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace imbored.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSavedActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedActivities_Activities_ActivitiesId",
                table: "UserSavedActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedActivities_AspNetUsers_SavedByUsersId",
                table: "UserSavedActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSavedActivities",
                table: "UserSavedActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserSavedActivities_SavedByUsersId",
                table: "UserSavedActivities");

            migrationBuilder.RenameColumn(
                name: "SavedByUsersId",
                table: "UserSavedActivities",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "ActivitiesId",
                table: "UserSavedActivities",
                newName: "ActivityId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserSavedActivities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSavedActivities",
                table: "UserSavedActivities",
                columns: new[] { "UserId", "ActivityId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedActivities_ActivityId",
                table: "UserSavedActivities",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedActivities_Activities_ActivityId",
                table: "UserSavedActivities",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedActivities_AspNetUsers_UserId",
                table: "UserSavedActivities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedActivities_Activities_ActivityId",
                table: "UserSavedActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedActivities_AspNetUsers_UserId",
                table: "UserSavedActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSavedActivities",
                table: "UserSavedActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserSavedActivities_ActivityId",
                table: "UserSavedActivities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSavedActivities");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "UserSavedActivities",
                newName: "SavedByUsersId");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "UserSavedActivities",
                newName: "ActivitiesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSavedActivities",
                table: "UserSavedActivities",
                columns: new[] { "ActivitiesId", "SavedByUsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedActivities_SavedByUsersId",
                table: "UserSavedActivities",
                column: "SavedByUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedActivities_Activities_ActivitiesId",
                table: "UserSavedActivities",
                column: "ActivitiesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedActivities_AspNetUsers_SavedByUsersId",
                table: "UserSavedActivities",
                column: "SavedByUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
