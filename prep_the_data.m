function data = prep_the_data(filenames,car_properties)
%% Load Data
obd_temp = readtable(filenames{1});
acc_temp = readtable(filenames{2});
gyr_temp = readtable(filenames{3});
mag_temp = readtable(filenames{4});
gps_temp = readtable(filenames{5});

%% OBD
obd = table2array(obd_temp);
seperate = 0;
for i=1:length(obd)
    if (obd(i,1) == -1)
        seperate = seperate + 1;
        count = 0;
    end
    if (count ~= 0)
        switch seperate
        case 1
            log.obd.speed.time(count,1) = obd(i,1);
            log.obd.speed.data(count,1) = obd(i,2);
        case 2
            log.obd.rpm.time(count,1) = obd(i,1);
            log.obd.rpm.data(count,1) = obd(i,2);
        case 3
            log.obd.fuel_rail_pressure.time(count,1) = obd(i,1);
            log.obd.fuel_rail_pressure.data(count,1) = obd(i,2);
        case 4
            log.obd.egr.time(count,1) = obd(i,1);
            log.obd.egr.data(count,1) = obd(i,2);
        case 5
            log.obd.coolent_temperature.time(count,1) = obd(i,1);
            log.obd.coolent_temperature.data(count,1) = obd(i,2);
        case 6
            log.obd.barometric_pressure.time(count,1) = obd(i,1);
            log.obd.barometric_pressure.data(count,1) = obd(i,2);
        case 7
            log.obd.intake_manifold_pressure.time(count,1) = obd(i,1);
            log.obd.intake_manifold_pressure.data(count,1) = obd(i,2);
        case 8
            log.obd.intake_air_temperature.time(count,1) = obd(i,1);
            log.obd.intake_air_temperature.data(count,1) = obd(i,2);
        case 9
            log.obd.mass_air_flow.time(count,1) = obd(i,1);
            log.obd.mass_air_flow.data(count,1) = obd(i,2);
        case 10
            log.obd.calculated_engine_load.time(count,1) = obd(i,1);
            log.obd.calculated_engine_load.data(count,1) = obd(i,2);
        case 11
            log.obd.accelerator_pedal_position_d.time(count,1) = obd(i,1);
            log.obd.accelerator_pedal_position_d.data(count,1) = obd(i,2);
        case 12
            log.obd.accelerator_pedal_position_e.time(count,1) = obd(i,1);
            log.obd.accelerator_pedal_position_e.data(count,1) = obd(i,2);
        otherwise
        end
    end
    count = count + 1;
end

%% IMU
% Accelerometer
accelerometer = table2array(acc_temp);
log.imu.accelerometer.time = accelerometer(:,1);
log.imu.accelerometer.x = accelerometer(:,2);
log.imu.accelerometer.y = accelerometer(:,3);
log.imu.accelerometer.z = accelerometer(:,4);
% Gyroscope
gyroscope = table2array(gyr_temp);
log.imu.gyroscope.time = gyroscope(:,1);
log.imu.gyroscope.x = gyroscope(:,2);
log.imu.gyroscope.y = gyroscope(:,3);
log.imu.gyroscope.z = gyroscope(:,4);
% Magnetometer
magnetometer = table2array(mag_temp);
log.imu.magnetometer.time = magnetometer(:,1);
log.imu.magnetometer.x = magnetometer(:,2);
log.imu.magnetometer.y = magnetometer(:,3);
log.imu.magnetometer.z = magnetometer(:,4);

%% GPS
gps = table2array(gps_temp);
log.gps.time = gps(:,1);
log.gps.latitude = gps(:,2);
log.gps.longitude = gps(:,3);
log.gps.height = gps(:,4);
log.gps.velocity = gps(:,5);
log.gps.direction = gps(:,6);
log.gps.horizontalAccuracy = gps(:,7);
log.gps.verticalAccuracy = gps(:,8);

%% Save
database.Make = car_properties{1};
database.Model = car_properties{2};
database.Year = car_properties{3};
database.Displacement = car_properties{4};
database.Fuel = car_properties{5};
database.Transmission = car_properties{6};
database.Weight = car_properties{7};
database.Tyres = car_properties{8};
database.Surface = car_properties{9};
database.Test = car_properties{10};
database.Log = log;

data = database;
end

