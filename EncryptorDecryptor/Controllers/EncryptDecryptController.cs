using EncryptorDecryptor.Impementation;
using EncryptorDecryptor.Impementation.Dto;
using EncryptorDecryptor.Impementation.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EncryptorDecryptor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptDecryptController : ControllerBase
    {
        public readonly IEncryptDecrypt _encryptDecryptService;
        public EncryptDecryptController(IEncryptDecrypt encryptDecryptService)
        {
            _encryptDecryptService = encryptDecryptService;
        }

        [ProducesResponseType(typeof(EncryptDto), StatusCodes.Status200OK)]
        [HttpPost("encrypt")]
        public IActionResult Encrypt(string payload)
        {
            try
            {
                var result = _encryptDecryptService.Encrypt(payload);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] DecryptDto dto)
        {
            try
            {
                var result = _encryptDecryptService.Decrypt(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sample-payload")]
        public IActionResult SamplePayload()
        {
            try
            {
                var result = _encryptDecryptService.SamplePayload();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(typeof(List<PeopleDto>), StatusCodes.Status200OK)]
        [HttpGet("sample-people-data")]
        public IActionResult SamplePeopleData()
        {
            try
            {
                var result = _encryptDecryptService.SamplePeopleData();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(typeof(PeopleDto), StatusCodes.Status200OK)]
        [HttpPost("people")]
        public IActionResult GetPeopleById(PeopleDataDto dto)
        {
            try
            {
                var result = _encryptDecryptService.GetPeopleById(dto);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}