using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;

namespace GylleneDroppen.Api.Repositories;

public class NewsletterRepository(AppDbContext context)
    : Repository<NewsletterSubscription>(context), INewsletterRepository;