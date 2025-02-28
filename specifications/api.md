# API Tasks for SalaPrenotazioni in c#

## Rooms API
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

## Reservations API
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

## POST /api/reservations
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

## PUT /api/reservations/[id]
// Request body - same as POST
interface UpdateReservationRequest {
  start_time?: string;
  end_time?: string;
  organizer?: string;
  notes?: string;
  status?: 'booked' | 'tentative' | 'cancelled';
}

### Test
- [ ] create the test cases for the API
- [ ] run the test cases
- [ ] fix the test cases if they fail