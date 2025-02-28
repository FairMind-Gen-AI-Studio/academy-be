# Sala Prenotazioni BE
This is a simple backend in C# that allows to manage a list of reservations for a meeting room. 

## Core Features

### Rooms
[ ] Add a Room
[ ] Remove a Room
[ ] Get all Rooms
[ ] Get a Room by ID
[ ] Get a Room by Name
[ ] Update a Room

### Reservations
[ ] Add a reservation
[ ] Remove a reservation
[ ] Get all reservations
[ ] Get all reservations for a specific day
[ ] Get all reservations for a specific user
[ ] Get all reservations for a specific day and user
[ ] Update a reservation


## Database Setup
[ ] Setup Sqlite
[ ] Set up database tables:
  ```sql
  -- Rooms table
  create table rooms (
    id uuid default uuid_generate_v4() primary key,
    name text not null,
    capacity integer not null,
    equipment text[] not null default '{}',
    created_at timestamp with time zone default timezone('utc'::text, now()) not null
  );

  -- Reservations table
  create table reservations (
    id uuid default uuid_generate_v4() primary key,
    room_id uuid references rooms(id) not null,
    start_time timestamp with time zone not null,
    end_time timestamp with time zone not null,
    status text not null check (status in ('booked', 'tentative', 'cancelled')),
    organizer text not null,
    notes text,
    created_at timestamp with time zone default timezone('utc'::text, now()) not null
  );
  ```

### Conflict Checking Logic
```sql
-- Check for overlapping reservations
SELECT id 
FROM reservations 
WHERE room_id = :room_id 
  AND status = 'booked'
  AND (
    (start_time <= :new_end_time AND end_time >= :new_start_time)
    OR (start_time >= :new_start_time AND start_time < :new_end_time)
    OR (end_time > :new_start_time AND end_time <= :new_end_time)
  )
  AND id != :current_reservation_id -- exclude current reservation for updates
```