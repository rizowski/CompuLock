class CreateAccountPrograms < ActiveRecord::Migration
  def change
    create_table :account_programs do |t|
      t.references :acount

      t.string :name
      t.datetime :lastrun
      t.integer :open_count

      t.timestamps
    end
  end
end
