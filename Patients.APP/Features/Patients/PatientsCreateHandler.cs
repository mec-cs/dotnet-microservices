using Core.APP.Models;
using MediatR;

namespace Patients.APP.Features;

public class PatientsCreateHandler: Request, IRequest<CommandResponse>
{
    public string Name { get; set; }
    
}