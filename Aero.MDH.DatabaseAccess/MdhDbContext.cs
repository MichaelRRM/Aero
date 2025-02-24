﻿using Aero.Base;
using Aero.MDH.DatabaseAccess.Models.Contract;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess;

public partial class MdhDbContext : DbContext
{
    private readonly IUserService _userService;
    public MdhDbContext(DbContextOptions<MdhDbContext> options, IUserService userService)
        : base(options)
    {
        _userService = userService;
    }
    private void BeforeSaveChanges()
    {
        var currentTime = DateTime.UtcNow;
        var userName = _userService.GetUserName();

        foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
        {
            if (item.Entity is not IAuditable productReferentialDbModel) continue;
            productReferentialDbModel.AuditCreateUser = userName;
            productReferentialDbModel.AuditCreateDate = currentTime;
        }
        foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
        {
            if (item.Entity is not IAuditable productReferentialDbModel) continue;
            productReferentialDbModel.AuditUpdateUser = userName;
            productReferentialDbModel.AuditUpdateDate = currentTime;
        }
    }

    public override int SaveChanges()
    {
        BeforeSaveChanges();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        BeforeSaveChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        BeforeSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        BeforeSaveChanges();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}