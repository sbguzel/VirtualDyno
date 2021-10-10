
[y_obd,ty_obd] = resample(database(1).Log.obd.speed.data,database(1).Log.obd.speed.time/1000,10,1,1);

[y_gps,ty_gps] = resample(database(1).Log.gps.velocity*3.6,database(1).Log.gps.time,10,1,1);

stem(ty_obd-47.5347,y_obd)
hold on
stem(ty_gps,y_gps)

stem(database(1).Log.obd.speed.time/1000-47.5347,database(1).Log.obd.speed.data)
hold on
plot(ty_obd,y_obd)
legend('obd speed','resample')

figure
plot(database(1).Log.gps.time,database(1).Log.gps.velocity*3.6)
hold on
plot(ty_gps,y_gps)

legend('gps speed','resample')

%%

[y_obd,ty_obd] = resample(database(2).Log.obd.speed.data,database(2).Log.obd.speed.time/1000,10,1,1);

[y_gps,ty_gps] = resample(database(2).Log.gps.velocity*3.6,database(2).Log.gps.time,10,1,1);

figure
plot(ty_obd,y_obd)
hold on
plot(ty_gps-7.2890,y_gps)

plot(database(2).Log.obd.speed.time/1000,database(2).Log.obd.speed.data)
hold on
plot(ty_obd,y_obd)
legend('obd speed','resample')

figure
plot(database(2).Log.gps.time,database(2).Log.gps.velocity*3.6)
hold on
plot(ty_gps,y_gps)

legend('gps speed','resample')