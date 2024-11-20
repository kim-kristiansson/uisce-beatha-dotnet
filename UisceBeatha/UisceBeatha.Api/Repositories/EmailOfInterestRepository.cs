using UisceBeatha.Api.Data;
using UisceBeatha.Api.Models;
using UisceBeatha.Api.Repositories.Interfaces;

namespace UisceBeatha.Api.Repositories;

public class EmailOfInterestRepository(AppDbContext context) :Repository<EmailOfInterest>(context), IEmailOfInterestRepository
{
    
}