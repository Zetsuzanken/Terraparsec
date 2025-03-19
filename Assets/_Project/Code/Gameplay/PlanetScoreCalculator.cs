using UnityEngine;

public static class PlanetScoreCalculator
{
    private const float ORBIT_EARTH = 1.0f;
    private const float ROTATION_EARTH = 24f;
    private const float ECC_EARTH = 0.0167f;
    private const float MASS_EARTH = 1f;
    private const float RADIUS_EARTH = 1f;
    private const float DENSITY_EARTH = 5.5f;
    private const float GRAVITY_EARTH = 9.81f;
    private const float PRESSURE_EARTH = 1.0f;
    private const float TEMP_EARTH = 288.15f;

    private const float ORBIT_MIN_BROAD = 0.5f;
    private const float ORBIT_MAX_BROAD = 3.4f;
    private const float ORBIT_MIN_HAB = 0.99f;
    private const float ORBIT_MAX_HAB = 1.7f;

    private const float ROTATION_MIN_BROAD = 12f;
    private const float ROTATION_MAX_BROAD = 48f;
    private const float ROTATION_MIN_HAB = 18f;
    private const float ROTATION_MAX_HAB = 36f;

    private const float ECC_MIN_BROAD = 0.0017f;
    private const float ECC_MAX_BROAD = 0.116f;
    private const float ECC_MIN_HAB = 0.0034f;
    private const float ECC_MAX_HAB = 0.058f;

    private const float MASS_MIN_BROAD = 0.25f;
    private const float MASS_MAX_BROAD = 20f;
    private const float MASS_MIN_HAB = 0.5f;
    private const float MASS_MAX_HAB = 10f;

    private const float RADIUS_MIN_BROAD = 0.4f;
    private const float RADIUS_MAX_BROAD = 4.4f;
    private const float RADIUS_MIN_HAB = 0.8f;
    private const float RADIUS_MAX_HAB = 2.2f;

    private const float PRESSURE_MIN_BROAD = 0.5f;
    private const float PRESSURE_MAX_BROAD = 2f;
    private const float PRESSURE_MIN_HAB = 0.75f;
    private const float PRESSURE_MAX_HAB = 1.5f;

    private const float TEMP_MIN_BROAD = 273.15f;
    private const float TEMP_MAX_BROAD = 323.15f;
    private const float TEMP_MIN_HAB = 280.65f;
    private const float TEMP_MAX_HAB = 298.15f;

    private const int TIER_1 = 100;
    private const int TIER_2 = 75;
    private const int TIER_3 = 50;
    private const int TIER_4 = 25;
    private const int TIER_5 = 0;

    public static int ScorePlanet(Planet p)
    {
        int total = 0;
        total += ScoreOrbit(p.orbitalDistance);
        total += ScoreRotation(p.rotationPeriod);
        total += ScoreEcc(p.eccentricity);
        total += ScoreMass(p.mass);
        total += ScoreRadius(p.radius);
        total += ScoreDensity(p);
        total += ScoreGravity(p);
        total += ScoreAtmosphere(p);
        total += ScorePressure(p.surfacePressure);
        total += ScoreTemperature(p.averageSurfaceTemperature);

        if (total > 1000)
        {
            total = 1000;
        }

        return total;
    }

    public static int ScoreOrbit(float orbitVal)
    {
        if (orbitVal is < ORBIT_MIN_BROAD or > ORBIT_MAX_BROAD)
        {
            return TIER_5;
        }

        float t1Low = Mathf.Max(ORBIT_EARTH * 0.9f, ORBIT_MIN_HAB);
        float t1High = Mathf.Min(ORBIT_EARTH * 1.1f, ORBIT_MAX_HAB);
        if (orbitVal >= t1Low && orbitVal <= t1High)
        {
            return TIER_1;
        }

        float t2Low = Mathf.Max(ORBIT_EARTH * 0.75f, ORBIT_MIN_HAB);
        float t2High = Mathf.Min(ORBIT_EARTH * 1.25f, ORBIT_MAX_HAB);
        return orbitVal >= t2Low && orbitVal <= t2High ? TIER_2 : orbitVal is >= ORBIT_MIN_HAB and <= ORBIT_MAX_HAB ? TIER_3 : TIER_4;
    }

    public static int ScoreRotation(float rotVal)
    {
        if (rotVal is < ROTATION_MIN_BROAD or > ROTATION_MAX_BROAD)
        {
            return TIER_5;
        }

        float t1Low = Mathf.Max(ROTATION_EARTH * 0.9f, ROTATION_MIN_HAB);
        float t1High = Mathf.Min(ROTATION_EARTH * 1.1f, ROTATION_MAX_HAB);
        if (rotVal >= t1Low && rotVal <= t1High)
        {
            return TIER_1;
        }

        float t2Low = Mathf.Max(ROTATION_EARTH * 0.75f, ROTATION_MIN_HAB);
        float t2High = Mathf.Min(ROTATION_EARTH * 1.25f, ROTATION_MAX_HAB);
        return rotVal >= t2Low && rotVal <= t2High ? TIER_2 : rotVal is >= ROTATION_MIN_HAB and <= ROTATION_MAX_HAB ? TIER_3 : TIER_4;
    }

    public static int ScoreEcc(float eccVal)
    {
        if (eccVal is < ECC_MIN_BROAD or > ECC_MAX_BROAD)
        {
            return TIER_5;
        }

        float t1Low = Mathf.Max(ECC_EARTH * 0.9f, ECC_MIN_HAB);
        float t1High = Mathf.Min(ECC_EARTH * 1.1f, ECC_MAX_HAB);
        if (eccVal >= t1Low && eccVal <= t1High)
        {
            return TIER_1;
        }

        float t2Low = Mathf.Max(ECC_EARTH * 0.75f, ECC_MIN_HAB);
        float t2High = Mathf.Min(ECC_EARTH * 1.25f, ECC_MAX_HAB);
        return eccVal >= t2Low && eccVal <= t2High ? TIER_2 : eccVal is >= ECC_MIN_HAB and <= ECC_MAX_HAB ? TIER_3 : TIER_4;
    }

    public static int ScoreMass(float massVal)
    {
        if (massVal is < MASS_MIN_BROAD or > MASS_MAX_BROAD)
        {
            return TIER_5;
        }

        float t1Low = Mathf.Max(MASS_EARTH * 0.9f, MASS_MIN_HAB);
        float t1High = Mathf.Min(MASS_EARTH * 1.1f, MASS_MAX_HAB);
        if (massVal >= t1Low && massVal <= t1High)
        {
            return TIER_1;
        }

        float t2Low = Mathf.Max(MASS_EARTH * 0.75f, MASS_MIN_HAB);
        float t2High = Mathf.Min(MASS_EARTH * 1.25f, MASS_MAX_HAB);
        return massVal >= t2Low && massVal <= t2High ? TIER_2 : massVal is >= MASS_MIN_HAB and <= MASS_MAX_HAB ? TIER_3 : TIER_4;
    }

    public static int ScoreRadius(float radiusVal)
    {
        if (radiusVal is < RADIUS_MIN_BROAD or > RADIUS_MAX_BROAD)
        {
            return TIER_5;
        }

        float t1Low = Mathf.Max(RADIUS_EARTH * 0.9f, RADIUS_MIN_HAB);
        float t1High = Mathf.Min(RADIUS_EARTH * 1.1f, RADIUS_MAX_HAB);
        if (radiusVal >= t1Low && radiusVal <= t1High)
        {
            return TIER_1;
        }

        float t2Low = Mathf.Max(RADIUS_EARTH * 0.75f, RADIUS_MIN_HAB);
        float t2High = Mathf.Min(RADIUS_EARTH * 1.25f, RADIUS_MAX_HAB);
        return radiusVal >= t2Low && radiusVal <= t2High ? TIER_2 : radiusVal is >= RADIUS_MIN_HAB and <= RADIUS_MAX_HAB ? TIER_3 : TIER_4;
    }

    public static int ScoreDensity(Planet p)
    {
        float pctDiff = Mathf.Abs(p.Density - DENSITY_EARTH) / DENSITY_EARTH * 100f;

        if (pctDiff <= 10f)
        {
            return TIER_1;
        }

        if (pctDiff <= 25f)
        {
            return TIER_2;
        }

        return pctDiff <= 50f ? TIER_3 : pctDiff <= 100f ? TIER_4 : TIER_5;
    }

    public static int ScoreGravity(Planet p)
    {
        float pctDiff = Mathf.Abs(p.SurfaceGravity - GRAVITY_EARTH) / GRAVITY_EARTH * 100f;

        if (pctDiff <= 10f)
        {
            return TIER_1;
        }

        if (pctDiff <= 25f)
        {
            return TIER_2;
        }

        return pctDiff <= 50f ? TIER_3 : pctDiff <= 100f ? TIER_4 : TIER_5;
    }

    public static int ScoreAtmosphere(Planet p) => !p.hasAtmosphere ? TIER_5 : p.generatedAsHabitable ? TIER_1 : TIER_5;

    public static int ScorePressure(float pVal)
    {
        if (pVal is < PRESSURE_MIN_BROAD or > PRESSURE_MAX_BROAD)
        {
            return TIER_5;
        }

        float t1Low = Mathf.Max(PRESSURE_EARTH * 0.9f, PRESSURE_MIN_HAB);
        float t1High = Mathf.Min(PRESSURE_EARTH * 1.1f, PRESSURE_MAX_HAB);
        if (pVal >= t1Low && pVal <= t1High)
        {
            return TIER_1;
        }

        float t2Low = Mathf.Max(PRESSURE_EARTH * 0.75f, PRESSURE_MIN_HAB);
        float t2High = Mathf.Min(PRESSURE_EARTH * 1.25f, PRESSURE_MAX_HAB);
        return pVal >= t2Low && pVal <= t2High ? TIER_2 : pVal is >= PRESSURE_MIN_HAB and <= PRESSURE_MAX_HAB ? TIER_3 : TIER_4;
    }

    public static int ScoreTemperature(float tVal)
    {
        if (tVal is < TEMP_MIN_BROAD or > TEMP_MAX_BROAD)
        {
            return TIER_5;
        }

        float t1Low = Mathf.Max(TEMP_EARTH * 0.9f, TEMP_MIN_HAB);
        float t1High = Mathf.Min(TEMP_EARTH * 1.1f, TEMP_MAX_HAB);
        if (tVal >= t1Low && tVal <= t1High)
        {
            return TIER_1;
        }

        float t2Low = Mathf.Max(TEMP_EARTH * 0.75f, TEMP_MIN_HAB);
        float t2High = Mathf.Min(TEMP_EARTH * 1.25f, TEMP_MAX_HAB);
        return tVal >= t2Low && tVal <= t2High ? TIER_2 : tVal is >= TEMP_MIN_HAB and <= TEMP_MAX_HAB ? TIER_3 : TIER_4;
    }
}
