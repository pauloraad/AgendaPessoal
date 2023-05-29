﻿// <auto-generated />
using System;
using Agenda.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Agenda.Server.Migrations
{
    [DbContext(typeof(AgendaContext))]
    [Migration("20230528182140_CriacaoInicial")]
    partial class CriacaoInicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Agenda.Server.Models.Agenda.Compromisso", b =>
                {
                    b.Property<int>("IdCompromisso")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCompromisso"), 1L, 1);

                    b.Property<int?>("ContatoIdContato")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataCompromisso")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<int>("FkIdContato")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("HorarioCompromisso")
                        .HasColumnType("time");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(MAX)");

                    b.HasKey("IdCompromisso");

                    b.HasIndex("ContatoIdContato");

                    b.ToTable("Compromisso");
                });

            modelBuilder.Entity("Agenda.Server.Models.Agenda.Contato", b =>
                {
                    b.Property<int>("IdContato")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContato"), 1L, 1);

                    b.Property<string>("Bairro")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cidade")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Complemento")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Estado")
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.Property<string>("RuaAvenida")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Telefone1")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Telefone2")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IdContato");

                    b.ToTable("Contato");
                });

            modelBuilder.Entity("Agenda.Server.Models.Agenda.Compromisso", b =>
                {
                    b.HasOne("Agenda.Server.Models.Agenda.Contato", "Contato")
                        .WithMany()
                        .HasForeignKey("ContatoIdContato");

                    b.Navigation("Contato");
                });
#pragma warning restore 612, 618
        }
    }
}
