using DiÆon.Attributes;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Domain.Interfaces;

namespace EmployeeManagementSystem.Application.Services;

[Scoped]
internal class PositionService(IPositionRepository positionRepository): IPositionService
{
    public async Task<IEnumerable<PositionDto>> GetTreeAsync()
    {
        var positions = await positionRepository.GetTreeAsync();
        return BuildTree(positions);
    }

    public async Task<(bool Success, string? ErrorMessage)> AddAsync(PositionDto positionDto)
    {
        try
        {
            if (positionDto.ParentId.HasValue)
            {
                var parent = await positionRepository.GetByIdAsync(positionDto.ParentId.Value);
                if (parent == null)
                    return (false, "Parent position not found");
            }

            var position = Domain.Entities.Position.Create(positionDto.Name, positionDto.ParentId);

            await positionRepository.AddAsync(position);
            return (true, null);
        }
        catch (ArgumentException ex)
        {
            return (false, ex.Message);
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while adding the position: {ex.Message}");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(int id)
    {
        try
        {
            var position = await positionRepository.GetByIdAsync(id);
            if (position == null)
                return (false, "Position not found");

            if (!position.CanBeDeleted())
                return (false, "Cannot delete position with children or employees. Please delete children and reassign employees first.");

            await positionRepository.DeleteAsync(id);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while deleting the position: {ex.Message}");
        }
    }

    private static List<PositionDto> BuildTree(IEnumerable<Domain.Entities.Position> positions)
    {
        var positionDict = positions.ToDictionary(p => p.Id, p => new PositionDto
        {
            Id = p.Id,
            Name = p.Name,
            ParentId = p.ParentId,
            Children = new List<PositionDto>()
        });

        var rootPositions = new List<PositionDto>();

        foreach (var position in positions)
        {
            var dto = positionDict[position.Id];
            if (position.ParentId.HasValue && positionDict.ContainsKey(position.ParentId.Value))
            {
                positionDict[position.ParentId.Value].Children ??= new List<PositionDto>();
                positionDict[position.ParentId.Value].Children!.Add(dto);
            }
            else
                rootPositions.Add(dto);
        }
        return rootPositions;
    }
}
