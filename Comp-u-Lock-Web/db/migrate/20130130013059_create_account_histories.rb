class CreateAccountHistories < ActiveRecord::Migration
  def change
    create_table :account_histories do |t|
      t.references :account

      t.string :domain
      t.string :url
      t.string :title
      t.datetime :last_visited
      t.integer :visit_count
      
      t.timestamps
    end
  end
end
