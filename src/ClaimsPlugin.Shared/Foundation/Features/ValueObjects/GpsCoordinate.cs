//using Foundation.Common.Persistence.Models;
//using Foundation.Common.Utilities;
//using Foundation.Features.Validation.Simple;
//using NetTopologySuite;
//using NetTopologySuite.Geometries;
//using Point = NetTopologySuite.Geometries.Point;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class GpsCoordinate : BaseValueObject
//{
//    protected GpsCoordinate()
//    {
//    }

//    private GpsCoordinate(Point value)
//    {
//        Value = value;
//    }

//    public decimal Latitude => Convert.ToDecimal(Value.Coordinate.Y);
//    public decimal Longitude => Convert.ToDecimal(Value.Coordinate.X);
//    public Point Value { get; private set; } = default!;
    
//    public static GpsCoordinate Create(decimal latitude, decimal longitude)
//    {
//        if (latitude.IsNotBetween(-90, 90, out string? latitudeNotBetweenErrorMessage))
//        {
//            Abort(latitudeNotBetweenErrorMessage);
//        }
        
//        if (longitude.IsNotBetween(-180, 180, out string? longitudeNotBetweenErrorMessage))
//        {
//            Abort(longitudeNotBetweenErrorMessage);
//        }
        
//        GeometryFactory? geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        
//        return new GpsCoordinate(geometryFactory.CreatePoint(new Coordinate(Convert.ToDouble(longitude.ToDecimalPlaces(6)), Convert.ToDouble(latitude.ToDecimalPlaces(6)))));
//    }

//    public decimal DistanceTo(GpsCoordinate targetCoordinate)
//    {
//        return DistanceTo(targetCoordinate, UnitOfLength.Miles);
//    }
    
//    public decimal DistanceTo(GpsCoordinate targetCoordinate, UnitOfLength unitOfLength)
//    {
//        // Value.Distance() returns cartesian degree
//        // 60 is the minutes
//        // 1.1515 is the number of statute miles of nautical miles
        
//        // may not be so accurate but is enough for our cases
//        return unitOfLength.ConvertFromDegrees(Value.Distance(targetCoordinate.Value));
//    }
    
//    public static implicit operator string(GpsCoordinate gpsCoordinate)
//    {
//        return gpsCoordinate.ToString();
//    }

//    public override string ToString()
//    {
//        return $"{Latitude}, {Longitude}";
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return Latitude;
//        yield return Longitude;
//    }
//}
