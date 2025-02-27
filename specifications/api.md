# API Tasks for SalaPrenotazioni with Sqlite in c#

## 1. Sqlite Setup
- [ ] Setup Sqlite
- [ ] Set up database tables:
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
- [ ] Create API keys and environment variables

## 2. API Routes Setup

### Rooms API
- [ ] `GET /api/rooms`
  - List all rooms
  - Support filtering by capacity and equipment
- [ ] `GET /api/rooms/[id]`
  - Get single room details
- [ ] `POST /api/rooms`
  - Create new room
- [ ] `PUT /api/rooms/[id]`
  - Update room details
- [ ] `DELETE /api/rooms/[id]`
  - Delete room

### Reservations API
- [ ] `GET /api/reservations`
  - List all reservations
  - Support filtering by date range and room
- [ ] `GET /api/reservations/[id]`
  - Get single reservation details
- [ ] `POST /api/reservations`
  - Create new reservation
  - Include conflict checking
- [ ] `PUT /api/reservations/[id]`
  - Update reservation
  - Include conflict checking
- [ ] `DELETE /api/reservations/[id]`
  - Cancel reservation

### POST /api/reservations
// Request body
interface CreateReservationRequest {
  room_id: string;
  start_time: string;  // ISO date string
  end_time: string;    // ISO date string
  organizer: string;
  notes?: string;
  status: 'booked' | 'tentative' | 'cancelled';
}

// Response
interface CreateReservationResponse {
  id: string;
  room_id: string;
  start_time: string;
  end_time: string;
  organizer: string;
  notes?: string;
  status: 'booked' | 'tentative' | 'cancelled';
  created_at: string;
}

### PUT /api/reservations/[id]
// Request body - same as POST
interface UpdateReservationRequest {
  start_time?: string;
  end_time?: string;
  organizer?: string;
  notes?: string;
  status?: 'booked' | 'tentative' | 'cancelled';
}

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

### Test
- [ ] create the test cases for the API
- [ ] run the test cases
- [ ] fix the test cases if they fail