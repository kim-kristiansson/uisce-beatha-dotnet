using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;

namespace GylleneDroppen.Api.Repositories;

public class AnalyticsRepository(AppDbContext context) : Repository<Analytics>(context), IAnalyticsRepository;