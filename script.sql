CREATE TABLE public.pessoas (
	id UUID PRIMARY KEY NOT NULL,
	apelido VARCHAR(32) UNIQUE NOT NULL,
	nome VARCHAR(100) NOT NULL,
	nascimento DATE NOT NULL,
	stack TEXT NULL,
	pesquisa TEXT GENERATED ALWAYS AS (
        LOWER(nome || apelido || stack)
    ) STORED
);

CREATE EXTENSION PG_TRGM;
CREATE INDEX CONCURRENTLY IF NOT EXISTS IDX_PESSOAS_BUSCA_TGRM ON PESSOAS USING GIST (pesquisa GIST_TRGM_OPS(SIGLEN=64));