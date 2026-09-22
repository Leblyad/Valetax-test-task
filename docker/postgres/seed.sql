-- Seed demo data for Valetax.
-- Apply AFTER APIs have started at least once (EF migrations create tables):
--
--   docker compose exec -T postgres psql -U postgres < docker/postgres/seed.sql
--
-- Hierarchy: Alice (root) <- Bob <- Carol
-- Schema: Alice/Bob = Linear, Carol = Fibonacci
-- Fixed GUIDs match postman/Valetax.postman_collection.json

\connect users_db

INSERT INTO users ("ExternalId", "Email", "Name") VALUES
  ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'alice@valetax.local', 'Alice'),
  ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'bob@valetax.local', 'Bob'),
  ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'carol@valetax.local', 'Carol')
ON CONFLICT ("ExternalId") DO NOTHING;

INSERT INTO partner_relations ("UserExternalId", "PartnerExternalId", "Level") VALUES
  ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 1),
  ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 1),
  ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 2)
ON CONFLICT ("UserExternalId", "Level") DO NOTHING;

\connect wallets_db

INSERT INTO wallets ("UserExternalId", "Balance", "SchemaType") VALUES
  ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 0, 'Linear'),
  ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 0, 'Linear'),
  ('cccccccc-cccc-cccc-cccc-cccccccccccc', 0, 'Fibonacci')
ON CONFLICT ("UserExternalId") DO NOTHING;
