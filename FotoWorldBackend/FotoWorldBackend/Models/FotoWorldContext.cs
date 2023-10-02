using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FotoWorldBackend.Models;

public partial class FotoWorldContext : DbContext
{
    public FotoWorldContext()
    {
    }

    public FotoWorldContext(DbContextOptions<FotoWorldContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FollowedOffer> FollowedOffers { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<OfferPhoto> OfferPhotos { get; set; }

    public virtual DbSet<Operator> Operators { get; set; }

    public virtual DbSet<OperatorRating> OperatorRatings { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FollowedOffer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.OfferId).HasColumnName("offerID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Offer).WithMany(p => p.FollowedOffers)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FollowedOffers_Offers");

            entity.HasOne(d => d.User).WithMany(p => p.FollowedOffers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FollowedOffers_Users");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OperatorId).HasColumnName("operatorID");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Operator).WithMany(p => p.Offers)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offers_Operators");
        });

        modelBuilder.Entity<OfferPhoto>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.OfferId).HasColumnName("offerID");
            entity.Property(e => e.PhotoId).HasColumnName("photoID");

            entity.HasOne(d => d.Offer).WithMany(p => p.OfferPhotos)
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferPhotos_Offers");

            entity.HasOne(d => d.Photo).WithMany(p => p.OfferPhotos)
                .HasForeignKey(d => d.PhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfferPhotos_Photos");
        });

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.Availability)
                .HasMaxLength(50)
                .HasColumnName("availability");
            entity.Property(e => e.DroneFilming).HasColumnName("droneFilming");
            entity.Property(e => e.DronePhoto).HasColumnName("dronePhoto");
            entity.Property(e => e.Filming).HasColumnName("filming");
            entity.Property(e => e.IsCompany).HasColumnName("isCompany");
            entity.Property(e => e.LocationCity)
                .HasMaxLength(150)
                .HasColumnName("locationCity");
            entity.Property(e => e.OperatingRadius).HasColumnName("operatingRadius");
            entity.Property(e => e.Photo).HasColumnName("photo");

            entity.HasOne(d => d.Account).WithMany(p => p.Operators)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operators_Users");
        });

        modelBuilder.Entity<OperatorRating>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .HasColumnName("comment");
            entity.Property(e => e.OperatorId).HasColumnName("operatorID");
            entity.Property(e => e.Stars).HasColumnName("stars");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Operator).WithMany(p => p.OperatorRatings)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperatorRatings_Operators");

            entity.HasOne(d => d.User).WithMany(p => p.OperatorRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperatorRatings_Users");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(200)
                .HasColumnName("photoURL");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.HashedPassword)
                .HasMaxLength(250)
                .HasColumnName("hashedPassword");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsOperator).HasColumnName("isOperator");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(250)
                .HasColumnName("passwordSalt");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
