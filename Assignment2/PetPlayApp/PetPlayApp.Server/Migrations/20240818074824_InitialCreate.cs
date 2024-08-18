﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetPlayApp.Server.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "TEXT", nullable: false),
					UserName = table.Column<string>(type: "TEXT", nullable: true),
					Password = table.Column<string>(type: "TEXT", nullable: true),
					Email = table.Column<string>(type: "TEXT", nullable: true),
					Age = table.Column<int>(type: "INTEGER", nullable: true),
					Bio = table.Column<string>(type: "TEXT", nullable: true),
					UserStatus = table.Column<int>(type: "INTEGER", nullable: true),
					Interest = table.Column<int>(type: "INTEGER", nullable: true),
					ProfilePictureData = table.Column<byte[]>(type: "BLOB", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Matches",
				columns: table => new
				{
					User1Id = table.Column<Guid>(type: "TEXT", nullable: false),
					User2Id = table.Column<Guid>(type: "TEXT", nullable: false),
					User1Response = table.Column<int>(type: "INTEGER", nullable: false),
					User2Response = table.Column<int>(type: "INTEGER", nullable: false),
					OverallStatus = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Matches", x => new { x.User1Id, x.User2Id });
					table.ForeignKey(
						name: "FK_Matches_Users_User1Id",
						column: x => x.User1Id,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Matches_Users_User2Id",
						column: x => x.User2Id,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Posts",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "TEXT", nullable: false),
					DateTimePosted = table.Column<DateTime>(type: "TEXT", nullable: false),
					PostCreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
					Description = table.Column<string>(type: "TEXT", nullable: true),
					ImageData = table.Column<byte[]>(type: "BLOB", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Posts", x => x.Id);
					table.ForeignKey(
						name: "FK_Posts_Users_PostCreatorId",
						column: x => x.PostCreatorId,
						principalTable: "Users",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "Stories",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "TEXT", nullable: false),
					DateTimePosted = table.Column<DateTime>(type: "TEXT", nullable: false),
					StoryCreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
					ImageData = table.Column<byte[]>(type: "BLOB", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Stories", x => x.Id);
					table.ForeignKey(
						name: "FK_Stories_Users_StoryCreatorId",
						column: x => x.StoryCreatorId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Comments",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "TEXT", nullable: false),
					Content = table.Column<string>(type: "TEXT", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
					PostId = table.Column<Guid>(type: "TEXT", nullable: false),
					UserId = table.Column<Guid>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Comments", x => x.Id);
					table.ForeignKey(
						name: "FK_Comments_Posts_PostId",
						column: x => x.PostId,
						principalTable: "Posts",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Comments_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "PostLike",
				columns: table => new
				{
					PostId = table.Column<Guid>(type: "TEXT", nullable: false),
					UserId = table.Column<Guid>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PostLike", x => new { x.PostId, x.UserId });
					table.ForeignKey(
						name: "FK_PostLike_Posts_PostId",
						column: x => x.PostId,
						principalTable: "Posts",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_PostLike_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "CommentLike",
				columns: table => new
				{
					CommentId = table.Column<Guid>(type: "TEXT", nullable: false),
					UserId = table.Column<Guid>(type: "TEXT", nullable: false),
					LikedCommentsId = table.Column<Guid>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CommentLike", x => new { x.CommentId, x.UserId });
					table.ForeignKey(
						name: "FK_CommentLike_Comments_CommentId",
						column: x => x.CommentId,
						principalTable: "Comments",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_CommentLike_Comments_LikedCommentsId",
						column: x => x.LikedCommentsId,
						principalTable: "Comments",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_CommentLike_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_CommentLike_LikedCommentsId",
				table: "CommentLike",
				column: "LikedCommentsId");

			migrationBuilder.CreateIndex(
				name: "IX_CommentLike_UserId",
				table: "CommentLike",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Comments_PostId",
				table: "Comments",
				column: "PostId");

			migrationBuilder.CreateIndex(
				name: "IX_Comments_UserId",
				table: "Comments",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Matches_User2Id",
				table: "Matches",
				column: "User2Id");

			migrationBuilder.CreateIndex(
				name: "IX_PostLike_UserId",
				table: "PostLike",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Posts_PostCreatorId",
				table: "Posts",
				column: "PostCreatorId");

			migrationBuilder.CreateIndex(
				name: "IX_Stories_StoryCreatorId",
				table: "Stories",
				column: "StoryCreatorId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CommentLike");

			migrationBuilder.DropTable(
				name: "Matches");

			migrationBuilder.DropTable(
				name: "PostLike");

			migrationBuilder.DropTable(
				name: "Stories");

			migrationBuilder.DropTable(
				name: "Comments");

			migrationBuilder.DropTable(
				name: "Posts");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
