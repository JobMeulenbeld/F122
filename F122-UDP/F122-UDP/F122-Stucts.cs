﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static F122_UDP.F122_Stucts;

namespace F122_UDP
{
    public class F122_Stucts
    {
        public enum PacketType : byte
        {
            Motion = 0, // Contains all motion data for player’s car – only sent while player is in control

            Session = 1,// Data about the session – track, time left

            LapData = 2,//  Data about all the lap times of cars in the session

            Event = 3, // Various notable events that happen during a session

            Participants = 4, // List of participants in the session, mostly relevant for multiplayer

            CarSetups = 5, // Packet detailing car setups for cars in the race

            CarTelemetry = 6,  // Telemetry data for all cars

            CarStatus = 7, //  Status data for all cars such as damage

            FinalClassification = 8, //Final classification confirmation at the end of a race

            LobbyInfo = 9, //Information about players in a multiplayer lobby

            CarDamage = 10, //Damage status of all cars

            SessionHistory = 11, //Lap and tyre data of the session
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0, Size = 24)]
        public struct PacketHeader
        {
            public UInt16 m_packetFormat;            // 2022
            public byte m_gameMajorVersion;        // Game major version - "X.00"
            public byte m_gameMinorVersion;        // Game minor version - "1.XX"
            public byte m_packetVersion;           // Version of this packet type, all start from 1
            public PacketType m_packetId;                // Identifier for the packet type, see below
            public UInt64 m_sessionUID;              // Unique identifier for the session
            public float m_sessionTime;             // Session timestamp
            public UInt32 m_frameIdentifier;         // Identifier for the frame the data was retrieved on
            public byte m_playerCarIndex;          // Index of player's car in the array
            public byte m_secondaryPlayerCarIndex; // Index of secondary player's car in the array (splitscreen) 255 if no second player
        };

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 60)]
        public struct CarMotionData
        {
            [FieldOffset(0)]
            public float m_worldPositionX;           // World space X position
            [FieldOffset(4)]
            public float m_worldPositionY;           // World space Y position
            [FieldOffset(8)]
            public float m_worldPositionZ;           // World space Z position
            [FieldOffset(12)]
            public float m_worldVelocityX;           // Velocity in world space X
            [FieldOffset(16)]
            public float m_worldVelocityY;           // Velocity in world space Y
            [FieldOffset(20)]
            public float m_worldVelocityZ;           // Velocity in world space Z
            [FieldOffset(24)]
            public UInt16 m_worldForwardDirX;         // World space forward X direction (normalised)
            [FieldOffset(26)]
            public UInt16 m_worldForwardDirY;         // World space forward Y direction (normalised)
            [FieldOffset(28)]
            public UInt16 m_worldForwardDirZ;         // World space forward Z direction (normalised)
            [FieldOffset(30)]
            public UInt16 m_worldRightDirX;           // World space right X direction (normalised)
            [FieldOffset(32)]
            public UInt16 m_worldRightDirY;           // World space right Y direction (normalised)
            [FieldOffset(34)]
            public UInt16 m_worldRightDirZ;           // World space right Z direction (normalised)
            [FieldOffset(36)]
            public float m_gForceLateral;            // Lateral G-Force component
            [FieldOffset(40)]
            public float m_gForceLongitudinal;       // Longitudinal G-Force component
            [FieldOffset(44)]
            public float m_gForceVertical;           // Vertical G-Force component
            [FieldOffset(48)]
            public float m_yaw;                      // Yaw angle in radians
            [FieldOffset(52)]
            public float m_pitch;                    // Pitch angle in radians
            [FieldOffset(56)]
            public float m_roll;                     // Roll angle in radians
        };

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 5)]
        public struct MarshalZone
        {
            [FieldOffset(0)]
            public float m_zoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
            [FieldOffset(4)]
            public sbyte m_zoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
        };

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 8)]
        public struct WeatherForecastSample
        {
            [FieldOffset(0)]
            public byte m_sessionType;              // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P, 5 = Q1
                                                    // 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ, 10 = R, 11 = R2
                                                    // 12 = R3, 13 = Time Trial
            [FieldOffset(1)]
            public byte m_timeOffset;               // Time in minutes the forecast is for
            [FieldOffset(2)]
            public byte m_weather;                  // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                                    // 3 = light rain, 4 = heavy rain, 5 = storm
            [FieldOffset(3)]
            public sbyte m_trackTemperature;         // Track temp. in degrees Celsius
            [FieldOffset(4)]
            public sbyte m_trackTemperatureChange;   // Track temp. change – 0 = up, 1 = down, 2 = no change
            [FieldOffset(5)]
            public sbyte m_airTemperature;           // Air temp. in degrees celsius
            [FieldOffset(6)]
            public sbyte m_airTemperatureChange;     // Air temp. change – 0 = up, 1 = down, 2 = no change
            [FieldOffset(7)]
            public byte m_rainPercentage;           // Rain percentage (0-100)
        };

        [StructLayout(LayoutKind.Sequential, Pack = 0, Size = 632)]
        public struct PacketSessionData
        {
            public PacketHeader m_header;         // Header

            public byte m_weather;                // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                                  // 3 = light rain, 4 = heavy rain, 5 = storm
            public sbyte m_trackTemperature;        // Track temp. in degrees celsius
            public sbyte m_airTemperature;          // Air temp. in degrees celsius
            public byte m_totalLaps;              // Total number of laps in this race
            public UInt16 m_trackLength;               // Track length in metres
            public byte m_sessionType;            // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P
                                                  // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
                                                  // 10 = R, 11 = R2, 12 = R3, 13 = Time Trial
            public sbyte m_trackId;                 // -1 for unknown, see appendix
            public byte m_formula;                    // Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2,
                                                      // 3 = F1 Generic, 4 = Beta, 5 = Supercars
                                                      // 6 = Esports, 7 = F2 2021
            public UInt16 m_sessionTimeLeft;       // Time left in session in seconds
            public UInt16 m_sessionDuration;       // Session duration in seconds
            public byte m_pitSpeedLimit;          // Pit speed limit in kilometres per hour
            public byte m_gamePaused;                // Whether the game is paused – network game only
            public byte m_isSpectating;           // Whether the player is spectating
            public byte m_spectatorCarIndex;      // Index of the car being spectated
            public byte m_sliProNativeSupport;    // SLI Pro support, 0 = inactive, 1 = active
            public byte m_numMarshalZones;            // Number of marshal zones to follow
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 21)]
            public MarshalZone[] m_marshalZones;             // List of marshal zones – max 21
            public byte m_safetyCarStatus;           // 0 = no safety car, 1 = full
                                                     // 2 = virtual, 3 = formation lap
            public byte m_networkGame;               // 0 = offline, 1 = online
            public byte m_numWeatherForecastSamples; // Number of weather samples to follow
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 56)]
            public WeatherForecastSample[] m_weatherForecastSamples;   // Array of weather forecast samples

            public byte m_forecastAccuracy;          // 0 = Perfect, 1 = Approximate
            public byte m_aiDifficulty;              // AI Difficulty rating – 0-110
            public UInt32 m_seasonLinkIdentifier;      // Identifier for season - persists across saves
            public UInt32 m_weekendLinkIdentifier;     // Identifier for weekend - persists across saves
            public UInt32 m_sessionLinkIdentifier;     // Identifier for session - persists across saves
            public byte m_pitStopWindowIdealLap;     // Ideal lap to pit on for current strategy (player)
            public byte m_pitStopWindowLatestLap;    // Latest lap to pit on for current strategy (player)
            public byte m_pitStopRejoinPosition;     // Predicted position to rejoin at (player)
            public byte m_steeringAssist;            // 0 = off, 1 = on
            public byte m_brakingAssist;             // 0 = off, 1 = low, 2 = medium, 3 = high
            public byte m_gearboxAssist;             // 1 = manual, 2 = manual & suggested gear, 3 = auto
            public byte m_pitAssist;                 // 0 = off, 1 = on
            public byte m_pitReleaseAssist;          // 0 = off, 1 = on
            public byte m_ERSAssist;                 // 0 = off, 1 = on
            public byte m_DRSAssist;                 // 0 = off, 1 = on
            public byte m_dynamicRacingLine;         // 0 = off, 1 = corners only, 2 = full
            public byte m_dynamicRacingLineType;     // 0 = 2D, 1 = 3D
            public byte m_gameMode;                  // Game mode id - see appendix
            public byte m_ruleSet;                   // Ruleset - see appendix
            public UInt32 m_timeOfDay;                 // Local time of day - minutes since midnight
            public byte m_sessionLength;             // 0 = None, 2 = Very Short, 3 = Short, 4 = Medium, 5 = Medium Long, 6 = Long, 7 = Full
        };
    }
}
