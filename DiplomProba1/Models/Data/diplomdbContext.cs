using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DiplomProba1.Models.Data
{
    public partial class diplomdbContext : DbContext
    {
        public diplomdbContext()
        {
        }

        public diplomdbContext(DbContextOptions<diplomdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Commenttext> Commenttexts { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Route> Routes { get; set; } = null!;
        public virtual DbSet<Statususerroute> Statususerroutes { get; set; } = null!;
        public virtual DbSet<Stausroute> Stausroutes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Userroute> Userroutes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseLazyLoadingProxies().UseMySql("server=localhost;user=root;database=diplomdb;password=Myidelm2002", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"));
            }


        }
        static diplomdbContext diplomdbContext1;
        public static diplomdbContext GetContext()
        {
            return diplomdbContext1 != null ? diplomdbContext1 : new diplomdbContext();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.IdCar)
                    .HasName("PRIMARY");

                entity.ToTable("car");

                entity.HasIndex(e => e.IdCommentCar, "Car_commenttext_fk_idx");

                entity.HasIndex(e => e.CarImage, "Car_image_fk_idx");

                entity.Property(e => e.IdCar)
                    .ValueGeneratedNever()
                    .HasColumnName("idCar");

                entity.Property(e => e.GosNumber).HasMaxLength(45);

                entity.Property(e => e.NameCar).HasMaxLength(45);

                entity.HasOne(d => d.CarImageNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CarImage)
                    .HasConstraintName("Car_image_fk");

                entity.HasOne(d => d.IdCommentCarNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.IdCommentCar)
                    .HasConstraintName("Car_commenttext_fk");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComment)
                    .HasName("PRIMARY");

                entity.ToTable("comments");

                entity.HasIndex(e => e.IdCommentText, "Comments_CommentText_FK_idx");

                entity.HasIndex(e => e.IdUserComment, "IDuserComment_user_idx");

                entity.HasIndex(e => e.IduserLeaveReview, "IDuserLR_user_idx");

                entity.Property(e => e.IdComment)
                    .ValueGeneratedNever()
                    .HasColumnName("idComment");

                entity.Property(e => e.Anonymity).HasColumnName("anonymity");

                entity.Property(e => e.Estimation)
                    .HasPrecision(2, 1)
                    .HasColumnName("estimation");

                entity.Property(e => e.IdCommentText).HasColumnName("idCommentText");

                entity.Property(e => e.IdUserComment).HasColumnName("idUserComment");

                entity.Property(e => e.IduserLeaveReview).HasColumnName("iduserLeaveReview");

                entity.HasOne(d => d.IdCommentTextNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdCommentText)
                    .HasConstraintName("Comments_CommentText_FK");

                entity.HasOne(d => d.IdUserCommentNavigation)
                    .WithMany(p => p.CommentIdUserCommentNavigations)
                    .HasForeignKey(d => d.IdUserComment)
                    .HasConstraintName("IDuserComment_user");

                entity.HasOne(d => d.IduserLeaveReviewNavigation)
                    .WithMany(p => p.CommentIduserLeaveReviewNavigations)
                    .HasForeignKey(d => d.IduserLeaveReview)
                    .HasConstraintName("IDuserLR_user");
            });

            modelBuilder.Entity<Commenttext>(entity =>
            {
                entity.HasKey(e => e.IdCommentText)
                    .HasName("PRIMARY");

                entity.ToTable("commenttext");

                entity.Property(e => e.IdCommentText)
                    .ValueGeneratedNever()
                    .HasColumnName("idCommentText");

                entity.Property(e => e.Text).HasColumnType("text");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.IdImages)
                    .HasName("PRIMARY");

                entity.ToTable("images");

                entity.Property(e => e.IdImages)
                    .ValueGeneratedNever()
                    .HasColumnName("idImages");

                entity.Property(e => e.Image1)
                    .HasColumnType("mediumblob")
                    .HasColumnName("Image");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PRIMARY");

                entity.ToTable("roles");

                entity.Property(e => e.IdRole)
                    .ValueGeneratedNever()
                    .HasColumnName("idRole");

                entity.Property(e => e.Role1)
                    .HasMaxLength(45)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.HasKey(e => e.IdRout)
                    .HasName("PRIMARY");

                entity.ToTable("routes");

                entity.HasIndex(e => e.IdCommentText, "Routes_CommentText_idx");

                entity.HasIndex(e => e.IdUser, "Routes_user_idx");

                entity.HasIndex(e => e.IdStatusRoute, "StatusRoute_idx");

                entity.Property(e => e.IdRout)
                    .ValueGeneratedNever()
                    .HasColumnName("idRout");

                entity.Property(e => e.BeginRoute)
                    .HasMaxLength(45)
                    .HasColumnName("beginRoute");

                entity.Property(e => e.DataTimeStart).HasColumnType("datetime");

                entity.Property(e => e.EndRoute)
                    .HasMaxLength(45)
                    .HasColumnName("endRoute");

                entity.Property(e => e.IdCommentText).HasColumnName("idCommentText");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.HasOne(d => d.IdCommentTextNavigation)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.IdCommentText)
                    .HasConstraintName("Routes_CommentText");

                entity.HasOne(d => d.IdStatusRouteNavigation)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.IdStatusRoute)
                    .HasConstraintName("StatusRoute");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Routes_user");
            });

            modelBuilder.Entity<Statususerroute>(entity =>
            {
                entity.HasKey(e => e.IdStatusUserRoute)
                    .HasName("PRIMARY");

                entity.ToTable("statususerroute");

                entity.Property(e => e.IdStatusUserRoute)
                    .ValueGeneratedNever()
                    .HasColumnName("idStatusUserRoute");

                entity.Property(e => e.StatusUserRoutecol).HasMaxLength(45);
            });

            modelBuilder.Entity<Stausroute>(entity =>
            {
                entity.HasKey(e => e.IdStausRoute)
                    .HasName("PRIMARY");

                entity.ToTable("stausroute");

                entity.Property(e => e.IdStausRoute)
                    .ValueGeneratedNever()
                    .HasColumnName("idStausRoute");

                entity.Property(e => e.StausRoutecol).HasMaxLength(45);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUsers)
                    .HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasIndex(e => e.CarId, "UserCar_car_idx");

                entity.HasIndex(e => e.UserImg, "User_Images_fk_idx");

                entity.HasIndex(e => e.IdCommentText, "Users_CommentText_idx");

                entity.HasIndex(e => e.IdRole, "Users_Roles_FK_idx");

                entity.Property(e => e.IdUsers)
                    .ValueGeneratedNever()
                    .HasColumnName("idUsers");

                entity.Property(e => e.Email).HasMaxLength(45);

                entity.Property(e => e.Estimation)
                    .HasPrecision(2, 1)
                    .HasColumnName("estimation");

                entity.Property(e => e.IdCommentText).HasColumnName("idCommentText");

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.Lastname).HasMaxLength(45);

                entity.Property(e => e.Login).HasMaxLength(45);

                entity.Property(e => e.Name).HasMaxLength(45);

                entity.Property(e => e.Password).HasMaxLength(45);

                entity.Property(e => e.Surname).HasMaxLength(45);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("UserCar_car");

                entity.HasOne(d => d.IdCommentTextNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdCommentText)
                    .HasConstraintName("Users_CommentText");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Users_Roles_FK");

                entity.HasOne(d => d.UserImgNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserImg)
                    .HasConstraintName("User_Images_fk");
            });

            modelBuilder.Entity<Userroute>(entity =>
            {
                entity.HasKey(e => e.IdUserroutes)
                    .HasName("PRIMARY");

                entity.ToTable("userroutes");

                entity.HasIndex(e => e.StatusUserRouteId, "StatusUserRoute_Status_idx");

                entity.HasIndex(e => e.IdRout, "Userroutes_Routes_FK_idx");

                entity.HasIndex(e => e.IdUser, "Userroutes_user_FK_idx");

                entity.Property(e => e.IdUserroutes)
                    .ValueGeneratedNever()
                    .HasColumnName("idUserroutes");

                entity.Property(e => e.IdRout).HasColumnName("idRout");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.HasOne(d => d.IdRoutNavigation)
                    .WithMany(p => p.Userroutes)
                    .HasForeignKey(d => d.IdRout)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Userroutes_Routes_FK");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Userroutes)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Userroutes_user_FK");

                entity.HasOne(d => d.StatusUserRoute)
                    .WithMany(p => p.Userroutes)
                    .HasForeignKey(d => d.StatusUserRouteId)
                    .HasConstraintName("StatusUserRoute_Status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
