using UisceBeatha.Api.Data;
using UisceBeatha.Api.Models;
using UisceBeatha.Api.Repositories.Interfaces;

namespace UisceBeatha.Api.Repositories;

public class NewsletterRepository(AppDbContext context)
    : Repository<NewsletterSubscription>(context), INewsletterRepository;