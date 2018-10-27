using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThingsToDoProject.Core.Interface;
using ThingsToDoProject.Core.Provider;
using ThingsToDoProject.Model;

namespace ThingsToDoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IGetOutsideData _getAllData;
        private readonly IGetData _getData;
        private readonly IGetLatitudeLongitude _getLatitudeLongitude;
        private readonly IGetInsideOutside _getInsideOutsideData;
        private readonly IGetPlaceData _getPlaceData;
        private readonly IGetDistanceTime _getDistanceTime;
        private readonly IGetSearch _getSearch;
        public DataController(IGetOutsideData getAllData, IGetData getData, IGetLatitudeLongitude getLatitudeLongitude, IGetInsideOutside getInsideOutsideData, IGetPlaceData getPlaceData, IGetDistanceTime getDistanceTime,IGetSearch getSearch)
        {
            _getAllData = getAllData;
            _getData = getData;
            _getLatitudeLongitude = getLatitudeLongitude;
            _getInsideOutsideData = getInsideOutsideData;
            _getPlaceData = getPlaceData;
            _getDistanceTime = getDistanceTime;
            _getSearch = getSearch;
        }

        //GET: api/Data/outsideAirport
        [HttpGet("outsideAirport/{DeparturePlace}/{ArrivalDateTime}/{DepartureDateTime}/{PointOfInterest}")]
        public async Task<IActionResult> GetOutsideData(String DeparturePlace, String ArrivalDateTime, String DepartureDateTime, String PointOfInterest)
        {
            var Data = await _getAllData.GetAllData(DeparturePlace, ArrivalDateTime, DepartureDateTime, PointOfInterest);
            if (Data != null)
                return Ok(Data);
            else
                return BadRequest("Data Not Found");
        }
        //GET: api/Data/search
        [HttpGet("search/{DeparturePlace}/{ArrivalDateTime}/{DepartureDateTime}/{PointOfInterest}")]
        public async Task<IActionResult> GetSearchData(String DeparturePlace, String ArrivalDateTime, String DepartureDateTime, String PointOfInterest)
        {
            var Data = await _getSearch.GetAllData(DeparturePlace, ArrivalDateTime, DepartureDateTime, PointOfInterest);
            if (Data != null)
                return Ok(Data);
            else
                return BadRequest("Data Not Found");
        }
        //GET: api/Data/insideAirport
        [HttpGet("insideAirport/{DeparturePlace}/{ArrivalDateTime}/{DepartureDateTime}/{PointOfInterest}")]

        //PointOfInterest is any Stores/Restorents...etc
        public async Task<IActionResult> GetInsideData(String DeparturePlace, String ArrivalDateTime, String DepartureDateTime, String PointOfInterest)
        {

            LocationAttributes Position = _getLatitudeLongitude.Get(DeparturePlace + "Airport");
            var Data = await _getData.GetData(Position, DeparturePlace, ArrivalDateTime, DepartureDateTime, PointOfInterest);
            if (Data != null)
                return Ok(Data);
            else
                return BadRequest("Not Found");
        }

        //GET: api/Data/InsideOutsideAirport
        [HttpGet("InsideOutsideAirport/{DeparturePlace}/{ArrivalDateTime}/{DepartureDateTime}/{PointOfInterest}")]
        public async Task<IActionResult> GetInsideOutsideData(String DeparturePlace, String ArrivalDateTime, String DepartureDateTime, String PointOfInterest)
        {
            LocationAttributes Position = _getLatitudeLongitude.Get(DeparturePlace + "Airport");
            var Data = await _getInsideOutsideData.GetInsideOutsideData(Position, DeparturePlace, ArrivalDateTime, DepartureDateTime, PointOfInterest);
            if (Data != null)
                return Ok(Data);
            else
                return BadRequest("Not Found");
        }

        
        //GET: api/Data/position
        [HttpGet("position/{Location}")]
        public LocationAttributes GetPosition(string Location)
        {
            LocationAttributes Position = _getLatitudeLongitude.Get(Location);
            return Position;
        }

        //GET: api/Data/place
        [HttpGet("place/{DeparturePlace}/{PlaceId}")]
        public async Task<IActionResult> GetInfoOfParticularPlace(string DeparturePlace,string PlaceID)
        {
            var Data = await _getPlaceData.GetPlaceData(DeparturePlace,PlaceID);
            if (Data != null)
                return Ok(Data);
            else
                return BadRequest("Not Found");
        }
        
    }
}
