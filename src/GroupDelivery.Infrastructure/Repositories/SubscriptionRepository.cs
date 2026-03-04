using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure;
using GroupDelivery.Infrastructure.Data;
using System;
using System.Data.Common;
using System.Linq;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly GroupDeliveryDbContext _db;

    public SubscriptionRepository(GroupDeliveryDbContext db)
    {
        _db = db;
    }

    public void Add(MerchantSubscription subscription)
    {
        _db.MerchantSubscriptions.Add(subscription);
        _db.SaveChanges();
    }

    public MerchantSubscription GetById(int id)
    {
        return _db.MerchantSubscriptions
            .FirstOrDefault(x => x.Id == id);
    }

    public MerchantSubscription GetByGatewayId(string gatewaySubscriptionId)
    {
        return _db.MerchantSubscriptions
            .FirstOrDefault(x => x.GatewaySubscriptionId == gatewaySubscriptionId);
    }

    public void Update(MerchantSubscription subscription)
    {
        _db.MerchantSubscriptions.Update(subscription);
        _db.SaveChanges();
    }
}