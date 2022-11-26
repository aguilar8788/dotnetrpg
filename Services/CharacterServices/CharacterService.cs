using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnetrpg.Dtos.Character;

namespace dotnetrpg.Services.CharacterServices
{
	public class CharacterService : ICharacterService
	{
		private static List<Character> characters = new List<Character> {
						new Character(),
						new Character { Id = 1, Name = "Sam" }
				};
		private readonly IMapper _mapper;

		public CharacterService(IMapper mapper)
		{
			_mapper = mapper;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
			Character character = _mapper.Map<Character>(newCharacter);
			character.Id = characters.Max(c => c.Id) + 1;
			characters.Add(character);
			serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
		{
			ServiceResponse<List<GetCharacterDTO>> response = new ServiceResponse<List<GetCharacterDTO>>();

			try
			{
				Character character = characters.First(c => c.Id == id);
				characters.Remove(character);
				response.Message = "Character has been deleted";
				response.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
		{
			return new ServiceResponse<List<GetCharacterDTO>> { Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList() };
		}

		public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDTO>();
			var character = characters.FirstOrDefault(c => c.Id == id);
			serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
		{
			ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();

			try
			{
				Character character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);

				character.Name = updateCharacter.Name;
				character.HitPoints = updateCharacter.HitPoints;
				character.Strength = updateCharacter.Strength;
				character.Defense = updateCharacter.Defense;
				character.Intelligence = updateCharacter.Intelligence;
				character.Class = updateCharacter.Class;

				response.Data = _mapper.Map<GetCharacterDTO>(character);
				response.Message = "Character has been updated";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}